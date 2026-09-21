namespace AccessManagement.Application.UseCases.ApprovalRoles.UpdateApprovalRole;

public sealed record UpdateApprovalRoleUseCaseOutput(
  Guid Id,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  IReadOnlyCollection<Guid> ProfileIds);
