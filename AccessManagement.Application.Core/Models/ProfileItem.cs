namespace AccessManagement.Application.Core.Models;

public sealed record ProfileItem(
  Guid Id,
  string AccountIdentifier,
  string Name,
  string? Description,
  bool Active,
  IReadOnlyCollection<Guid> PermissionIds,
  DateTime CreatedAt,
  DateTime? UpdatedAt);
