namespace AccessManagement.Application.UseCases.ApprovalRoles.ListApprovalRoles;

public sealed record ListApprovalRolesUseCaseInput(string AccountIdentifier, Guid AuthorityId) : IUseCaseInput
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

    if (AuthorityId == Guid.Empty)
    {
      errors.Add("AuthorityId is required.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
