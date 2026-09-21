using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Entities;
using AccessManagement.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class ApprovalRoleRepository(
  AccessManagementDbContext dbContext,
  ILogger<ApprovalRoleRepository> logger) : IApprovalRoleRepository
{
  public async Task<IReadOnlyCollection<ApprovalRoleItem>> GetByAccountAsync(
    string accountIdentifier,
    Guid authorityId,
    CancellationToken cancellationToken = default)
  {
    var rows = await (
      from role in dbContext.AuthorityApprovalRoles.AsNoTracking()
      join link in dbContext.ApprovalRoleProfiles.AsNoTracking()
        on role.Id equals link.AuthorityApprovalRoleId
      join profile in dbContext.Profiles.AsNoTracking()
        on link.ProfileId equals profile.Id
      join account in dbContext.Accounts.AsNoTracking()
        on profile.AccountId equals account.Id
      where role.AuthorityId == authorityId && account.Identifier == accountIdentifier
      select new
      {
        Role = role,
        link.ProfileId
      }).ToArrayAsync(cancellationToken);

    return rows
      .GroupBy(item => item.Role.Id)
      .Select(group =>
      {
        var role = group.First().Role;

        return new ApprovalRoleItem(
          role.Id,
          role.AuthorityId,
          role.ValueLimit,
          role.IsUnlimitedValueLimit,
          role.MinApprovers,
          role.Active,
          group.Select(item => item.ProfileId).Distinct().ToArray(),
          role.CreatedAt,
          role.UpdatedAt);
      })
      .OrderBy(item => item.IsUnlimitedValueLimit)
      .ThenBy(item => item.ValueLimit)
      .ToArray();
  }

  public async Task<AuthorityApprovalRole?> InsertAsync(
    string accountIdentifier,
    AuthorityApprovalRole role,
    IReadOnlyCollection<Guid> profileIds,
    CancellationToken cancellationToken = default)
  {
    if (!await dbContext.Authorities.AnyAsync(
          item => item.Id == role.AuthorityId,
          cancellationToken))
    {
      return null;
    }

    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);
    if (accountId is null)
    {
      return null;
    }

    await EnsureProfilesAsync(accountId.Value, profileIds, cancellationToken);
    await dbContext.AuthorityApprovalRoles.AddAsync(role, cancellationToken);

    await dbContext.ApprovalRoleProfiles.AddRangeAsync(
      profileIds
        .Distinct()
        .Select(profileId => new ApprovalRoleProfile
        {
          AuthorityApprovalRoleId = role.Id,
          AuthorityApprovalRole = role,
          ProfileId = profileId,
          Active = true,
          CreatedBy = role.CreatedBy
        }),
      cancellationToken);

    await SaveAsync(nameof(InsertAsync), cancellationToken);

    return role;
  }

  public async Task<AuthorityApprovalRole?> UpdateAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    decimal? valueLimit,
    bool unlimited,
    short minApprovers,
    IReadOnlyCollection<Guid> profileIds,
    string? updatedBy,
    CancellationToken cancellationToken = default)
  {
    Validate(valueLimit, unlimited, minApprovers);

    var role = await GetOwnedRoleAsync(accountIdentifier, roleId, cancellationToken);

    if (role is null || role.AuthorityId != authorityId)
    {
      return null;
    }

    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);
    if (accountId is null)
    {
      return null;
    }

    await EnsureProfilesAsync(accountId.Value, profileIds, cancellationToken);

    role.ValueLimit = valueLimit;
    role.IsUnlimitedValueLimit = unlimited;
    role.MinApprovers = minApprovers;
    role.UpdatedAt = DateTime.UtcNow;
    role.UpdatedBy = Normalize(updatedBy);

    var links = await dbContext.ApprovalRoleProfiles
      .Where(item => item.AuthorityApprovalRoleId == roleId)
      .ToArrayAsync(cancellationToken);

    var ids = profileIds.Distinct().ToArray();

    foreach (var link in links)
    {
      link.Active = ids.Contains(link.ProfileId);
    }

    foreach (var profileId in ids.Except(links.Select(item => item.ProfileId)))
    {
      await dbContext.ApprovalRoleProfiles.AddAsync(
        new ApprovalRoleProfile
        {
          AuthorityApprovalRoleId = roleId,
          ProfileId = profileId,
          Active = true,
          CreatedBy = Normalize(updatedBy)
        },
        cancellationToken);
    }

    await SaveAsync(nameof(UpdateAsync), cancellationToken);

    return role;
  }

  public async Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    bool active,
    bool confirmPrivilegeElevation,
    string? updatedBy,
    CancellationToken cancellationToken = default)
  {
    var role = await GetOwnedRoleAsync(accountIdentifier, roleId, cancellationToken);

    if (role is null || role.AuthorityId != authorityId)
    {
      return false;
    }

    if (!active && role.Active && !confirmPrivilegeElevation)
    {
      var profiles = await dbContext.ApprovalRoleProfiles
        .Where(item => item.AuthorityApprovalRoleId == roleId && item.Active)
        .Select(item => item.ProfileId)
        .ToArrayAsync(cancellationToken);

      var profilesWithAnotherRole = await dbContext.ApprovalRoleProfiles
        .Where(item =>
          profiles.Contains(item.ProfileId) &&
          item.Active &&
          item.AuthorityApprovalRoleId != roleId)
        .Join(
          dbContext.AuthorityApprovalRoles.Where(
            item => item.Active && item.AuthorityId == authorityId),
          link => link.AuthorityApprovalRoleId,
          other => other.Id,
          (link, other) => link.ProfileId)
        .Distinct()
        .ToArrayAsync(cancellationToken);

      if (profiles.Except(profilesWithAnotherRole).Any())
      {
        throw new InvalidConfigurationException(
          "Disabling this role makes at least one approval profile unlimited. Explicit confirmation is required.");
      }
    }

    role.Active = active;
    role.UpdatedAt = DateTime.UtcNow;
    role.UpdatedBy = Normalize(updatedBy);

    await SaveAsync(nameof(SetActivationAsync), cancellationToken);

    return true;
  }

  private async Task EnsureProfilesAsync(
    Guid accountId,
    IReadOnlyCollection<Guid> profileIds,
    CancellationToken cancellationToken)
  {
    var ids = profileIds.Distinct().ToArray();

    if (ids.Length == 0)
    {
      throw new InvalidConfigurationException("At least one profile is required.");
    }

    var profileCount = await dbContext.Profiles.CountAsync(
      item => ids.Contains(item.Id) && item.AccountId == accountId,
      cancellationToken);

    if (profileCount != ids.Length)
    {
      throw new InvalidConfigurationException(
        "Every profile must belong to the account from the route.");
    }
  }

  private async Task<AuthorityApprovalRole?> GetOwnedRoleAsync(
    string accountIdentifier,
    Guid roleId,
    CancellationToken cancellationToken)
  {
    var role = await dbContext.AuthorityApprovalRoles.SingleOrDefaultAsync(
      item => item.Id == roleId,
      cancellationToken);

    if (role is null)
    {
      return null;
    }

    var accountIds = await dbContext.ApprovalRoleProfiles
      .Where(item => item.AuthorityApprovalRoleId == roleId)
      .Join(
        dbContext.Profiles,
        link => link.ProfileId,
        profile => profile.Id,
        (link, profile) => profile.AccountId)
      .Distinct()
      .ToArrayAsync(cancellationToken);

    var accountId = await ResolveAccountIdAsync(accountIdentifier, cancellationToken);

    return accountId is not null && accountIds.Length == 1 && accountIds[0] == accountId.Value
      ? role
      : null;
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
        "[ApprovalRoleRepository][SaveAsync] A uniqueness conflict occurred. Operation: {Operation}.",
        method);

      throw new DataConflictException(
        "The approval role conflicts with an existing record.",
        exception);
    }
    catch (Exception exception) when (exception is SqlException or DbUpdateException)
    {
      logger.LogError(
        exception,
        "[ApprovalRoleRepository][SaveAsync] SQL Server communication failed. Operation: {Operation}.",
        method);

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  private static void Validate(
    decimal? valueLimit,
    bool unlimited,
    short minApprovers)
  {
    if (minApprovers <= 0 ||
        unlimited && valueLimit is not null ||
        !unlimited && valueLimit is null or <= 0)
    {
      throw new InvalidConfigurationException(
        "The approval role threshold is invalid.");
    }
  }

  private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
