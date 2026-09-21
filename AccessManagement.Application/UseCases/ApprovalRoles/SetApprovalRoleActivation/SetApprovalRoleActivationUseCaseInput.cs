namespace AccessManagement.Application.UseCases.ApprovalRoles.SetApprovalRoleActivation;

public sealed record SetApprovalRoleActivationUseCaseInput(
  string AccountIdentifier,
  Guid AuthorityId,
  Guid RoleId,
  bool Active,
  bool ConfirmPrivilegeElevation,
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

    if (AuthorityId == Guid.Empty)
    {
      errors.Add("AuthorityId is required.");
    }

    if (RoleId == Guid.Empty)
    {
      errors.Add("RoleId is required.");
    }

    if (UpdatedBy?.Trim().Length > 150)
    {
      errors.Add("UpdatedBy must contain at most 150 characters.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
