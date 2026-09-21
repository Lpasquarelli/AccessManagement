namespace AccessManagement.Application.UseCases.Profiles.SetProfilePermissions;

public sealed record SetProfilePermissionsUseCaseInput(
  string AccountIdentifier,
  Guid ProfileId,
  Guid[] PermissionIds) : IUseCaseInput
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

    if (PermissionIds is null || PermissionIds.Any(id => id == Guid.Empty))
    {
      errors.Add("PermissionIds is invalid.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
