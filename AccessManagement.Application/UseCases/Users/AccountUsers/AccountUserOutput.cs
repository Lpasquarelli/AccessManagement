using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.UseCases.Users.AccountUsers;

public sealed record AccountUserOutput(
  Guid UserAccountId,
  Guid UserId,
  string Name,
  bool Active,
  bool IsHolder,
  bool IsMaster,
  IReadOnlyCollection<AccountUserProfileOutput> Profiles)
{
  public static AccountUserOutput From(AccountUserItem item) =>
    new(
      item.UserAccountId,
      item.UserId,
      item.Name,
      item.Active,
      item.IsHolder,
      item.IsMaster,
      item.Profiles.Select(profile => new AccountUserProfileOutput(
        profile.Name,
        profile.Description)).ToArray());
}

public sealed record AccountUserProfileOutput(
  string Name,
  string? Description);
