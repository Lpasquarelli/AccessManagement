namespace AccessManagement.Application.UseCases.Profiles.UpdateProfile;

public sealed record UpdateProfileUseCaseInput(
  string AccountIdentifier,
  Guid ProfileId,
  string Name,
  string? Description,
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

    if (string.IsNullOrWhiteSpace(Name))
    {
      errors.Add("Name is required.");
    }
    else if (Name.Trim().Length > 150)
    {
      errors.Add("Name must contain at most 150 characters.");
    }

    if (Description?.Trim().Length > 500)
    {
      errors.Add("Description must contain at most 500 characters.");
    }

    if (UpdatedBy?.Trim().Length > 150)
    {
      errors.Add("UpdatedBy must contain at most 150 characters.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
