namespace AccessManagement.Application.UseCases.Users.AccountUsers.AddUserToAccount;

public sealed record AddUserToAccountUseCaseOutput(
  Guid UserAccountId,
  Guid UserId,
  string AccountIdentifier,
  bool Active,
  bool IsMaster,
  bool IsHolder);
