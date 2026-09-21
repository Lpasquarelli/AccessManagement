using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class ProfilePermissionFactory
{
  public ProfilePermission Create(Profile profile, Guid permissionId)
  {
    var profilePermission = Create(profile.Id, permissionId);
    profilePermission.Profile = profile;
    return profilePermission;
  }

  public ProfilePermission Create(Guid profileId, Guid permissionId) =>
    new()
    {
      ProfileId = profileId,
      PermissionId = permissionId
    };
}
