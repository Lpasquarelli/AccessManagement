namespace AccessManagement.Application.Core.Models;

public sealed record ApprovalRoleItem(
  Guid Id,
  Guid AuthorityId,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  bool Active,
  IReadOnlyCollection<Guid> ProfileIds,
  DateTime CreatedAt,
  DateTime? UpdatedAt);
