namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountActivation;

public sealed record SetUserAccountActivationUseCaseOutput(
  Guid UserAccountId,
  bool Active);
