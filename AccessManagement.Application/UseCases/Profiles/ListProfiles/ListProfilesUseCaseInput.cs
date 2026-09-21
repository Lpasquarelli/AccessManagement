namespace AccessManagement.Application.UseCases.Profiles.ListProfiles;

public sealed record ListProfilesUseCaseInput(string AccountIdentifier) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput() =>
    string.IsNullOrWhiteSpace(AccountIdentifier)
      ? (false, ["AccountIdentifier is required."])
      : (true, []);
}
