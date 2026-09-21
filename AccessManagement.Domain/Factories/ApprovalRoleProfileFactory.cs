using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class ApprovalRoleProfileFactory
{
  public ApprovalRoleProfile Create(
    Guid authorityApprovalRoleId,
    Guid profileId,
    string? createdBy) =>
    new()
    {
      AuthorityApprovalRoleId = authorityApprovalRoleId,
      ProfileId = profileId,
      CreatedBy = FactoryValueNormalizer.Optional(createdBy),
      Active = true
    };
}
