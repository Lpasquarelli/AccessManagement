namespace AccessManagement.Application.Core.Models;

public sealed record EffectivePermission(Guid Id, string Description, string Group);

public sealed record EffectiveProfile(
  Guid Id,
  string Name,
  string? Description,
  IReadOnlyCollection<EffectivePermission> Permissions);

public sealed record UserAccountAccess(
  string Identifier,
  Guid ContextId,
  string Context,
  string Currency,
  Guid UserAccountId,
  bool IsHolder,
  bool IsMaster,
  IReadOnlyCollection<EffectiveProfile> Profiles,
  IReadOnlyCollection<EffectivePermission> EffectivePermissions);

public sealed record UserAccessSnapshot(
  Guid UserId,
  string Name,
  string AuthenticationId,
  IReadOnlyCollection<UserAccountAccess> Accounts);

public sealed record ApplicableApprovalPolicy(
  Guid Id,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  IReadOnlyCollection<Guid> EligibleProfileIds);

public sealed record AccountAccessEvaluation(
  Guid UserId,
  Guid UserAccountId,
  string AccountIdentifier,
  Guid ContextId,
  string Context,
  string Currency,
  bool IsHolder,
  bool IsMaster,
  Guid RequestedPermissionId,
  bool HasRequestedPermission,
  bool CanApprove,
  bool CanApproveAlone,
  bool CanCreateApproval,
  bool IsUnlimitedApprover,
  IReadOnlyCollection<EffectiveProfile> Profiles,
  IReadOnlyCollection<EffectivePermission> EffectivePermissions,
  IReadOnlyCollection<ApplicableApprovalPolicy> ApprovalPolicies,
  IReadOnlyCollection<Guid> EligibleApprovalPolicyIds,
  ApplicableApprovalPolicy? SelectedPolicy,
  bool IsEligibleForSelectedPolicy);
