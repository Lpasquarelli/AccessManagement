namespace AccessManagement.Application.UseCases.Profiles.CreateProfile;

public sealed record CreateProfileUseCaseInput(
  string AccountIdentifier,
  string Name,
  string? Description,
  Guid[] PermissionIds,
  string? CreatedBy) : IUseCaseInput
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

    if (PermissionIds is null || PermissionIds.Any(id => id == Guid.Empty))
    {
      errors.Add("PermissionIds is invalid.");
    }

    if (CreatedBy?.Trim().Length > 150)
    {
      errors.Add("CreatedBy must contain at most 150 characters.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
