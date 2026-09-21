namespace AccessManagement.Application.Core.Models;

public sealed record AccountUserItem(
  Guid UserAccountId,
  Guid UserId,
  string Name,
  bool Active,
  bool IsHolder,
  bool IsMaster,
  IReadOnlyCollection<AccountUserProfileItem> Profiles);

public sealed record AccountUserProfileItem(
  string Name,
  string? Description);
