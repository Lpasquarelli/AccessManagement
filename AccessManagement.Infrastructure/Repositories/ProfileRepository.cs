using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Entities;
using AccessManagement.Infrastructure.Context;
using AccessManagement.Domain.Factories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class ProfileRepository(
  AccessManagementDbContext dbContext,
  ProfilePermissionFactory profilePermissionFactory,
  ILogger<ProfileRepository> logger) : IProfileRepository
{
  public async Task<IReadOnlyCollection<ProfileItem>> GetByAccountAsync(
    string accountIdentifier,
    CancellationToken cancellationToken = default)
  {
    var profiles = await (
      from profile in dbContext.Profiles.AsNoTracking()
      join account in dbContext.Accounts.AsNoTracking() on profile.AccountId equals account.Id
      where account.Identifier == accountIdentifier
      select profile)
      .AsNoTracking()
      .OrderBy(item => item.Name)
      .ToArrayAsync(cancellationToken);

    var ids = profiles.Select(item => item.Id).ToArray();

    var permissions = await dbContext.ProfilePermissions
      .AsNoTracking()
      .Where(item => ids.Contains(item.ProfileId))
      .ToArrayAsync(cancellationToken);

    return profiles
      .Select(profile => Map(
        profile,
        accountIdentifier,
        permissions
          .Where(item => item.ProfileId == profile.Id)
          .Select(item => item.PermissionId)))
      .ToArray();
  }

  public async Task<ProfileItem?> InsertAsync(
    string accountIdentifier,
    Profile profile,
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken = default)
  {
    var accountId = await dbContext.Accounts
      .Where(item => item.Identifier == accountIdentifier && item.Active)
      .Select(item => (Guid?)item.Id)
      .SingleOrDefaultAsync(cancellationToken);

    if (accountId is null)
      return null;

    profile.AccountId = accountId.Value;
    await EnsurePermissionsAsync(permissionIds, cancellationToken);
    await dbContext.Profiles.AddAsync(profile, cancellationToken);
    await dbContext.ProfilePermissions.AddRangeAsync(
      permissionIds
        .Distinct()
        .Select(permissionId => profilePermissionFactory.Create(profile, permissionId)),
      cancellationToken);
    await SaveAsync(nameof(InsertAsync), cancellationToken);
    return Map(profile, accountIdentifier, permissionIds);
  }

  public async Task<ProfileItem?> UpdateAsync(
    string accountIdentifier,
    Guid profileId,
    string name,
    string? description,
    string? updatedBy,
    CancellationToken cancellationToken = default)
  {
    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);
    var profile = accountId is null
      ? null
      : await dbContext.Profiles.SingleOrDefaultAsync(
        item => item.AccountId == accountId.Value && item.Id == profileId,
        cancellationToken);

    if (profile is null)
    {
      return null;
    }

    profile.Name = name.Trim();
    profile.Description = Normalize(description);
    profile.UpdatedBy = Normalize(updatedBy);
    profile.UpdatedAt = DateTime.UtcNow;
    await SaveAsync(nameof(UpdateAsync), cancellationToken);
    var permissions = await dbContext.ProfilePermissions
      .AsNoTracking()
      .Where(item => item.ProfileId == profileId)
      .Select(item => item.PermissionId)
      .ToArrayAsync(cancellationToken);

    return Map(profile, accountIdentifier, permissions);
  }

  public async Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid profileId,
    bool active,
    string? updatedBy,
    CancellationToken cancellationToken = default)
  {
    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);
    var profile = accountId is null
      ? null
      : await dbContext.Profiles.SingleOrDefaultAsync(
        item => item.AccountId == accountId.Value && item.Id == profileId,
        cancellationToken);

    if (profile is null)
    {
      return false;
    }

    profile.Active = active;
    profile.UpdatedBy = Normalize(updatedBy);
    profile.UpdatedAt = DateTime.UtcNow;
    await SaveAsync(nameof(SetActivationAsync), cancellationToken);
    return true;
  }

  public async Task<bool> ReplacePermissionsAsync(
    string accountIdentifier,
    Guid profileId,
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken = default)
  {
    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);

    if (accountId is null || !await dbContext.Profiles.AnyAsync(
          item => item.AccountId == accountId.Value && item.Id == profileId,
          cancellationToken))
    {
      return false;
    }

    await EnsurePermissionsAsync(permissionIds, cancellationToken);

    var currentPermissions = await dbContext.ProfilePermissions
      .Where(item => item.ProfileId == profileId)
      .ToArrayAsync(cancellationToken);

    dbContext.ProfilePermissions.RemoveRange(currentPermissions);

    await dbContext.ProfilePermissions.AddRangeAsync(
      permissionIds
        .Distinct()
        .Select(permissionId => new ProfilePermission
        {
          ProfileId = profileId,
          PermissionId = permissionId
        }),
      cancellationToken);
    await SaveAsync(nameof(ReplacePermissionsAsync), cancellationToken);
    return true;
  }

  private async Task EnsurePermissionsAsync(
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken)
  {
    var ids = permissionIds.Distinct().ToArray();
    var permissionCount = await dbContext.Permissions.CountAsync(
      item => ids.Contains(item.Id),
      cancellationToken);

    if (permissionCount != ids.Length)
    {
      throw new InvalidConfigurationException("One or more permissions do not exist.");
    }
  }

  private async Task<Guid?> ResolveAccountIdAsync(
    string accountIdentifier,
    CancellationToken cancellationToken) =>
    await dbContext.Accounts
      .Where(item => item.Identifier == accountIdentifier)
      .Select(item => (Guid?)item.Id)
      .SingleOrDefaultAsync(cancellationToken);

  private async Task SaveAsync(string method, CancellationToken cancellationToken)
  {
    try
    {
      await dbContext.SaveChangesAsync(cancellationToken);
    }
    catch (DbUpdateException exception) when
      (exception.InnerException is SqlException sql && sql.Number is 2601 or 2627)
    {
      logger.LogWarning(
        exception,
        "[ProfileRepository][SaveAsync] A uniqueness conflict occurred. Operation: {Operation}.",
        method);

      throw new DataConflictException(
        "The profile conflicts with an existing record.",
        exception);
    }
    catch (Exception exception) when (exception is SqlException or DbUpdateException)
    {
      logger.LogError(
        exception,
        "[ProfileRepository][SaveAsync] SQL Server communication failed. Operation: {Operation}.",
        method);

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

  private static ProfileItem Map(
    Profile profile,
    string accountIdentifier,
    IEnumerable<Guid> permissionIds) =>
    new(
      profile.Id,
      accountIdentifier,
      profile.Name,
      profile.Description,
      profile.Active,
      permissionIds.Distinct().ToArray(),
      profile.CreatedAt,
      profile.UpdatedAt);
}
