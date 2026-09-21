namespace AccessManagement.Application.UseCases.Accesses.GetUserAccesses;

public sealed record GetUserAccessesUseCaseInput(string AuthenticationId) : IUseCaseInput
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

    return (errors.Count == 0, errors.ToArray());
  }
}
