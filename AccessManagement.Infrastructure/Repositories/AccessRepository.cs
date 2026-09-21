using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Authorization;
using AccessManagement.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class AccessRepository(
  AccessManagementDbContext dbContext,
  ILogger<AccessRepository> logger) : IAccessRepository
{
  public async Task<UserAccessSnapshot?> GetByAuthenticationIdAsync(
    string authenticationId,
    CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[AccessRepository][GetByAuthenticationIdAsync] Querying effective user access.");

    try
    {
      var user = await dbContext.Users
        .AsNoTracking()
        .SingleOrDefaultAsync(
          item => item.AuthenticationId == authenticationId && item.Active,
          cancellationToken);

      if (user is null)
      {
        return null;
      }

      var accountRows = await (
        from userAccount in dbContext.UserAccounts.AsNoTracking()
        join account in dbContext.Accounts.AsNoTracking()
          on userAccount.AccountId equals account.Id
        join context in dbContext.AccessContexts.AsNoTracking()
          on account.ContextId equals context.Id
        where userAccount.UserId == user.Id && userAccount.Active && account.Active
        orderby account.Identifier
        select new
        {
          UserAccount = userAccount,
          Account = account,
          Context = context
        }).ToArrayAsync(cancellationToken);

      var accounts = new List<UserAccountAccess>(accountRows.Length);
      foreach (var row in accountRows)
      {
        var profiles = await LoadProfilesAsync(
          row.UserAccount.Id,
          row.Account.Id,
          row.Context.Id,
          cancellationToken);

        var effectivePermissions = row.UserAccount.IsMaster
          ? await LoadContextPermissionsAsync(row.Context.Id, cancellationToken)
          : profiles
            .SelectMany(item => item.Permissions)
            .DistinctBy(item => item.Id)
            .OrderBy(item => item.Group)
            .ThenBy(item => item.Description)
            .ToArray();

        accounts.Add(new UserAccountAccess(
          row.Account.Identifier,
          row.Context.Id,
          row.Context.Name,
          row.Context.Currency,
          row.UserAccount.Id,
          row.UserAccount.IsHolder,
          row.UserAccount.IsMaster,
          profiles,
          effectivePermissions));
      }

      return new UserAccessSnapshot(
        user.Id,
        user.Name,
        user.AuthenticationId,
        accounts);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[AccessRepository][GetByAuthenticationIdAsync] SQL Server communication failed while querying user access.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  public async Task<AccountAccessEvaluation?> EvaluateAsync(
    string authenticationId,
    string accountIdentifier,
    Guid permissionId,
    Guid authorityId,
    decimal? amount,
    CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[AccessRepository][EvaluateAsync] Evaluating user rules for an account.");

    try
    {
      var snapshot = await GetByAuthenticationIdAsync(authenticationId, cancellationToken);
      var account = snapshot?.Accounts.SingleOrDefault(
        item => item.Identifier == accountIdentifier);

      if (snapshot is null || account is null)
      {
        return null;
      }

      var accountId = await dbContext.Accounts
        .Where(item => item.Identifier == accountIdentifier)
        .Select(item => (Guid?)item.Id)
        .SingleOrDefaultAsync(cancellationToken);

      if (accountId is null)
      {
        return null;
      }

      var profileIds = account.Profiles.Select(item => item.Id).ToArray();
      var policyRows = await (
        from role in dbContext.AuthorityApprovalRoles.AsNoTracking()
        join link in dbContext.ApprovalRoleProfiles.AsNoTracking()
          on role.Id equals link.AuthorityApprovalRoleId
        join profile in dbContext.Profiles.AsNoTracking()
          on link.ProfileId equals profile.Id
        where role.AuthorityId == authorityId
          && role.Active
          && link.Active
          && profile.Active
          && profile.AccountId == accountId.Value
        select new
        {
          Role = role,
          ProfileId = profile.Id
        }).ToArrayAsync(cancellationToken);

      var policies = policyRows
        .GroupBy(item => item.Role.Id)
        .Select(group =>
        {
          var role = group.First().Role;

          return new ApplicableApprovalPolicy(
            role.Id,
            role.ValueLimit,
            role.IsUnlimitedValueLimit,
            role.MinApprovers,
            group.Select(item => item.ProfileId).Distinct().ToArray());
        })
        .OrderBy(item => item.IsUnlimitedValueLimit)
        .ThenBy(item => item.ValueLimit)
        .ToArray();

      var approvalProfileIds = account.Profiles
        .Where(profile => profile.Permissions.Any(
          permission => PermissionCatalog.ApprovalPermissions.Contains(permission.Id)))
        .Select(profile => profile.Id)
        .ToArray();

      var eligiblePolicyIds = account.IsMaster
        ? policies.Select(item => item.Id).ToArray()
        : policyRows
          .Where(item => profileIds.Contains(item.ProfileId))
          .Select(item => item.Role.Id)
          .Distinct()
          .ToArray();

      var profilesWithPolicy = policyRows
        .Where(item => profileIds.Contains(item.ProfileId))
        .Select(item => item.ProfileId)
        .Distinct()
        .ToArray();

      var canApprove = account.IsMaster || approvalProfileIds.Length > 0;
      var hasRequestedPermission = account.EffectivePermissions.Any(
        item => item.Id == permissionId);
      var isUnlimitedApprover = canApprove && approvalProfileIds.Except(profilesWithPolicy).Any();
      ApplicableApprovalPolicy? selectedPolicy = null;

      if (amount is > 0)
      {
        selectedPolicy = policies
          .Where(item => item.IsUnlimitedValueLimit || item.ValueLimit >= amount)
          .OrderBy(item => item.IsUnlimitedValueLimit)
          .ThenBy(item => item.ValueLimit)
          .FirstOrDefault();
      }

      var isEligibleForSelectedPolicy = selectedPolicy is not null &&
        eligiblePolicyIds.Contains(selectedPolicy.Id);
      var canApproveAlone = canApprove &&
        isEligibleForSelectedPolicy &&
        selectedPolicy?.MinApprovers == 1;

      return new AccountAccessEvaluation(
        snapshot.UserId,
        account.UserAccountId,
        account.Identifier,
        account.ContextId,
        account.Context,
        account.Currency,
        account.IsHolder,
        account.IsMaster,
        permissionId,
        hasRequestedPermission,
        canApprove,
        canApproveAlone,
        !account.IsHolder && hasRequestedPermission,
        isUnlimitedApprover,
        account.Profiles,
        account.EffectivePermissions,
        policies,
        eligiblePolicyIds,
        selectedPolicy,
        isEligibleForSelectedPolicy);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[AccessRepository][EvaluateAsync] SQL Server communication failed while evaluating user rules.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }

  private async Task<IReadOnlyCollection<EffectiveProfile>> LoadProfilesAsync(
    Guid userAccountId,
    Guid accountId,
    Guid contextId,
    CancellationToken cancellationToken)
  {
    var profiles = await (
      from assignment in dbContext.UserAccountProfiles.AsNoTracking()
      join profile in dbContext.Profiles.AsNoTracking()
        on assignment.ProfileId equals profile.Id
      where assignment.UserAccountId == userAccountId &&
        assignment.Active &&
        profile.Active &&
        profile.AccountId == accountId
      orderby profile.Name
      select profile).ToArrayAsync(cancellationToken);

    if (profiles.Length == 0)
    {
      return [];
    }

    var profileIds = profiles.Select(item => item.Id).ToArray();
    var permissions = await (
      from link in dbContext.ProfilePermissions.AsNoTracking()
      join permission in dbContext.Permissions.AsNoTracking()
        on link.PermissionId equals permission.Id
      where profileIds.Contains(link.ProfileId)
      select new
      {
        link.ProfileId,
        Permission = permission
      }).ToArrayAsync(cancellationToken);

    var groups = await dbContext.PermissionGroups.AsNoTracking()
      .Where(item => item.ContextId == contextId)
      .ToDictionaryAsync(item => item.PermissionId, item => item.Description, cancellationToken);

    return profiles
      .Select(profile => new EffectiveProfile(
        profile.Id,
        profile.Name,
        profile.Description,
        permissions
          .Where(item => item.ProfileId == profile.Id)
          .Select(item => new EffectivePermission(
            item.Permission.Id,
            item.Permission.Description,
            groups.GetValueOrDefault(item.Permission.Id, "Outras Permissões")))
          .OrderBy(item => item.Group)
          .ThenBy(item => item.Description)
          .ToArray()))
      .ToArray();
  }

  private async Task<IReadOnlyCollection<EffectivePermission>> LoadContextPermissionsAsync(
    Guid contextId,
    CancellationToken cancellationToken)
  {
    return await (
      from permissionGroup in dbContext.PermissionGroups.AsNoTracking()
      join permission in dbContext.Permissions.AsNoTracking()
        on permissionGroup.PermissionId equals permission.Id
      where permissionGroup.ContextId == contextId
      orderby permissionGroup.Description, permission.Description
      select new EffectivePermission(
        permission.Id,
        permission.Description,
        permissionGroup.Description))
      .ToArrayAsync(cancellationToken);
  }
}
