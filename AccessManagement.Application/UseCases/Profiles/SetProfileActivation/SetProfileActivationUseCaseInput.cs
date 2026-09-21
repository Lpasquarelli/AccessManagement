namespace AccessManagement.Application.UseCases.Profiles.SetProfileActivation;

public sealed record SetProfileActivationUseCaseInput(
  string AccountIdentifier,
  Guid ProfileId,
  bool Active,
  string? UpdatedBy) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(AccountIdentifier))
    {
      errors.Add("AccountIdentifier is required.");
    }
    else if (AccountIdentifier.Trim().Length > 50)
    {
      errors.Add("AccountIdentifier must contain at most 50 characters.");
    }

    if (ProfileId == Guid.Empty)
    {
      errors.Add("ProfileId is required.");
    }

    if (UpdatedBy?.Trim().Length > 150)
    {
      errors.Add("UpdatedBy must contain at most 150 characters.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
