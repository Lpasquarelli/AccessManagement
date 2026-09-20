namespace AccessManagement.Application.UseCases.Users.DeleteUser;

public sealed record DeleteUserUseCaseInput(
  Guid Id,
  string? UpdatedBy) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = new List<string>();

    if (Id == Guid.Empty)
      errors.Add("Id must be a non-empty GUID.");

    if (!string.IsNullOrWhiteSpace(UpdatedBy) && UpdatedBy.Trim().Length > 150)
      errors.Add("UpdatedBy must contain at most 150 characters.");

    return (errors.Count == 0, errors.ToArray());
  }
}
