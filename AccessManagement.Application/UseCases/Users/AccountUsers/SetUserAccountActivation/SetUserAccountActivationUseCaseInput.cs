namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountActivation;

public sealed record SetUserAccountActivationUseCaseInput(
  string AccountIdentifier,
  Guid UserAccountId,
  bool Active) : IUseCaseInput
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

    return (errors.Count == 0, errors.ToArray());
  }
}
