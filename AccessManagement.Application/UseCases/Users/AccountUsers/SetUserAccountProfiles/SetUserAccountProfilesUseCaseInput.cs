namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountProfiles;

public sealed record SetUserAccountProfilesUseCaseInput(
  string AccountIdentifier,
  Guid UserAccountId,
  Guid[] ProfileIds,
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

    if (UserAccountId == Guid.Empty)
    {
      errors.Add("UserAccountId is required.");
    }

    if (ProfileIds is null || ProfileIds.Any(id => id == Guid.Empty))
    {
      errors.Add("ProfileIds is invalid.");
    }

    if (UpdatedBy?.Trim().Length > 150)
    {
      errors.Add("UpdatedBy must contain at most 150 characters.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
