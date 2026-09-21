using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Entities;
using AccessManagement.Domain.Factories;
using AccessManagement.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class UserAccountRepository(
  AccessManagementDbContext dbContext,
  UserAccountFactory userAccountFactory,
  UserAccountProfileFactory userAccountProfileFactory,
  ILogger<UserAccountRepository> logger) : IUserAccountRepository
{
  public async Task<IReadOnlyCollection<AccountUserItem>> GetByAccountAsync(
    string accountIdentifier,
    int page,
    int pageSize,
    CancellationToken cancellationToken = default)
  {
    var rows = await (from link in dbContext.UserAccounts.AsNoTracking()
                      join user in dbContext.Users.AsNoTracking() on link.UserId equals user.Id
                      join account in dbContext.Accounts.AsNoTracking() on link.AccountId equals account.Id
                      where account.Identifier == accountIdentifier
                      orderby user.Name
                      select new
                      {
                        Link = link,
                        user.Name
                      })
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToArrayAsync(cancellationToken);

    var ids = rows.Select(item => item.Link.Id).ToArray();

    var assignments = await dbContext.UserAccountProfiles.AsNoTracking()
      .Where(item => ids.Contains(item.UserAccountId) && item.Active)
      .ToArrayAsync(cancellationToken);

    var profileIds = assignments.Select(item => item.ProfileId).Distinct().ToArray();
    var profiles = await dbContext.Profiles.AsNoTracking()
      .Where(item => profileIds.Contains(item.Id))
      .Select(item => new
      {
        item.Id,
        item.Name,
        item.Description
      })
      .ToArrayAsync(cancellationToken);

    return rows
      .Select(item => new AccountUserItem(
        item.Link.Id,
        item.Link.UserId,
        item.Name,
        item.Link.Active,
        item.Link.IsHolder,
        item.Link.IsMaster,
        assignments
          .Where(link => link.UserAccountId == item.Link.Id)
          .Join(
            profiles,
            assignment => assignment.ProfileId,
            profile => profile.Id,
            (_, profile) => new AccountUserProfileItem(profile.Name, profile.Description))
          .ToArray()))
      .ToArray();
  }

  public async Task<UserAccount?> CreateAsync(
    string accountIdentifier,
    User user,
    IReadOnlyCollection<Guid> profileIds,
    string? createdBy,
    CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[UserAccountRepository][CreateAsync] Creating an operator and its account relationship.");

    var account = await dbContext.Accounts.SingleOrDefaultAsync(
      item => item.Identifier == accountIdentifier && item.Active,
      cancellationToken);

    if (account is null)
    {
      return null;
    }

    var ids = profileIds.Distinct().ToArray();
    await EnsureProfilesAsync(account.Id, ids, cancellationToken);

    await dbContext.Users.AddAsync(user, cancellationToken);

    var link = userAccountFactory.Create(
      user,
      account.Id,
      isMaster: false,
      isHolder: false);

    await dbContext.UserAccounts.AddAsync(link, cancellationToken);
    await ReplaceProfilesAsync(link, ids, createdBy, cancellationToken);
    await SaveAsync(nameof(CreateAsync), cancellationToken);

    return link;
  }

  public async Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid userAccountId,
    bool active,
    CancellationToken cancellationToken = default)
  {
    var accountId = await dbContext.Accounts
      .Where(item => item.Identifier == accountIdentifier)
      .Select(item => (Guid?)item.Id)
      .SingleOrDefaultAsync(cancellationToken);

    var link = accountId is null
      ? null
      : await dbContext.UserAccounts.SingleOrDefaultAsync(
        item => item.AccountId == accountId.Value && item.Id == userAccountId,
        cancellationToken);

    if (link is null)
    {
      return false;
    }

    link.Active = active;
    await SaveAsync(nameof(SetActivationAsync), cancellationToken);
    return true;
  }

  public async Task<bool> ReplaceProfilesAsync(
    string accountIdentifier,
    Guid userAccountId,
    IReadOnlyCollection<Guid> profileIds,
    string? updatedBy,
    CancellationToken cancellationToken = default)
  {
    var accountId = await dbContext.Accounts
      .Where(item => item.Identifier == accountIdentifier)
      .Select(item => (Guid?)item.Id)
      .SingleOrDefaultAsync(cancellationToken);

    var link = accountId is null
      ? null
      : await dbContext.UserAccounts.SingleOrDefaultAsync(
        item => item.Id == userAccountId && item.AccountId == accountId.Value,
        cancellationToken);

    if (link is null)
    {
      return false;
    }

    var ids = profileIds.Distinct().ToArray();

    if (link.IsMaster)
    {
      throw new InvalidConfigurationException(
        "Master account users do not require profiles.");
    }

    await EnsureProfilesAsync(accountId!.Value, ids, cancellationToken);
    await ReplaceProfilesAsync(link, ids, updatedBy, cancellationToken);
    await SaveAsync(nameof(ReplaceProfilesAsync), cancellationToken);

    return true;
  }

  private async Task EnsureProfilesAsync(
    Guid accountId,
    IReadOnlyCollection<Guid> profileIds,
    CancellationToken cancellationToken)
  {
    var profileCount = await dbContext.Profiles.CountAsync(
      item => profileIds.Contains(item.Id) &&
        item.AccountId == accountId &&
        item.Active,
      cancellationToken);

    if (profileCount != profileIds.Count)
    {
      throw new InvalidConfigurationException(
        "Every profile must be active and belong to the account from the route.");
    }
  }

  private async Task ReplaceProfilesAsync(
    UserAccount link,
    IReadOnlyCollection<Guid> profileIds,
    string? actor,
    CancellationToken cancellationToken)
  {
    var assignments = await dbContext.UserAccountProfiles
      .Where(item => item.UserAccountId == link.Id)
      .ToArrayAsync(cancellationToken);

    foreach (var assignment in assignments)
    {
      assignment.Active = profileIds.Contains(assignment.ProfileId);
      assignment.UpdatedAt = DateTime.UtcNow;
      assignment.UpdatedBy = Normalize(actor);
    }

    foreach (var profileId in profileIds.Except(assignments.Select(item => item.ProfileId)))
    {
      await dbContext.UserAccountProfiles.AddAsync(
        userAccountProfileFactory.Create(link, profileId, actor),
        cancellationToken);
    }
  }

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
        "[UserAccountRepository][SaveAsync] A uniqueness conflict occurred. Operation: {Operation}.",
        method);

      throw new DataConflictException(
        "A user with the same authentication identifier or account relationship already exists.",
        exception);
    }
    catch (Exception exception) when (exception is SqlException or DbUpdateException)
    {
      logger.LogError(
        exception,
        "[UserAccountRepository][SaveAsync] SQL Server communication failed. Operation: {Operation}.",
        method);

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
