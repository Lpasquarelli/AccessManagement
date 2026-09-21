using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class UserAccountProfileFactory
{
  public UserAccountProfile Create(UserAccount userAccount, Guid profileId, string? createdBy)
  {
    var assignment = Create(userAccount.Id, profileId, createdBy);
    assignment.UserAccount = userAccount;
    return assignment;
  }

  public UserAccountProfile Create(Guid userAccountId, Guid profileId, string? createdBy) =>
    new()
    {
      UserAccountId = userAccountId,
      ProfileId = profileId,
      CreatedBy = FactoryValueNormalizer.Optional(createdBy),
      Active = true
    };
}
