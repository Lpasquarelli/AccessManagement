using AccessManagement.Application.Core.Models;
namespace AccessManagement.Application.UseCases.ApprovalRoles;

public sealed record ApprovalRoleOutput(
  Guid Id,
  Guid AuthorityId,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  bool Active,
  IReadOnlyCollection<Guid> ProfileIds,
  DateTime CreatedAt,
  DateTime? UpdatedAt)
{
  public static ApprovalRoleOutput From(ApprovalRoleItem item) =>
    new(
      item.Id,
      item.AuthorityId,
      item.ValueLimit,
      item.IsUnlimitedValueLimit,
      item.MinApprovers,
      item.Active,
      item.ProfileIds,
      item.CreatedAt,
      item.UpdatedAt);
}
