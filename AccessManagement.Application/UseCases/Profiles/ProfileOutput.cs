using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.UseCases.Profiles;

public sealed record ProfileOutput(
  Guid Id,
  string AccountIdentifier,
  string Name,
  string? Description,
  bool Active,
  IReadOnlyCollection<Guid> PermissionIds,
  DateTime CreatedAt,
  DateTime? UpdatedAt)
{
  public static ProfileOutput From(ProfileItem profile) =>
    new(
      profile.Id,
      profile.AccountIdentifier,
      profile.Name,
      profile.Description,
      profile.Active,
      profile.PermissionIds,
      profile.CreatedAt,
      profile.UpdatedAt);
}
