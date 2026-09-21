using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class UserAccountFactory
{
  public UserAccount Create(User user, Guid accountId, bool isMaster, bool isHolder)
  {
    var userAccount = Create(user.Id, accountId, isMaster, isHolder);
    userAccount.User = user;
    return userAccount;
  }

  public UserAccount Create(Guid userId, Guid accountId, bool isMaster, bool isHolder) =>
    new()
    {
      UserId = userId,
      AccountId = accountId,
      IsMaster = isMaster,
      IsHolder = isHolder,
      Active = true
    };
}
