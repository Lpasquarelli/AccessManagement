namespace AccessManagement.Application.UseCases.Accesses.EvaluateAccountAccess;

public sealed record EvaluateAccountAccessUseCaseInput(
  string AuthenticationId,
  string AccountIdentifier,
  Guid PermissionId,
  Guid AuthorityId,
  decimal? Amount) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(AuthenticationId))
    {
      errors.Add("AuthenticationId is required.");
    }
    else if (AuthenticationId.Trim().Length > 150)
    {
      errors.Add("AuthenticationId must contain at most 150 characters.");
    }

    if (string.IsNullOrWhiteSpace(AccountIdentifier))
    {
      errors.Add("AccountIdentifier is required.");
    }
    else if (AccountIdentifier.Trim().Length > 50)
    {
      errors.Add("AccountIdentifier must contain at most 50 characters.");
    }

    if (PermissionId == Guid.Empty)
    {
      errors.Add("PermissionId is required.");
    }

    if (AuthorityId == Guid.Empty)
    {
      errors.Add("AuthorityId is required.");
    }

    if (Amount is <= 0)
    {
      errors.Add("Amount must be greater than zero when informed.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
