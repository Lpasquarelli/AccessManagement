namespace AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;

public sealed record CreateApprovalRoleUseCaseOutput(
  Guid Id,
  Guid AuthorityId,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  bool Active,
  IReadOnlyCollection<Guid> ProfileIds);
