namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountProfiles;

public sealed record SetUserAccountProfilesUseCaseOutput(
  Guid UserAccountId,
  IReadOnlyCollection<Guid> ProfileIds);
