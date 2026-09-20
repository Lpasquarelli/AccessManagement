namespace AccessManagement.Application.UseCases.Users.GetUserById;

public sealed record GetUserByIdUseCaseInput(Guid Id) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = new List<string>();

    if (Id == Guid.Empty)
      errors.Add("Id must be a non-empty GUID.");

    return (errors.Count == 0, errors.ToArray());
  }
}
