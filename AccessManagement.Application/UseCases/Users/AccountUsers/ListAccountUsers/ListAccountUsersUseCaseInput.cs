namespace AccessManagement.Application.UseCases.Users.AccountUsers.ListAccountUsers;

public sealed record ListAccountUsersUseCaseInput(
  string AccountIdentifier,
  int Page = 1,
  int PageSize = 20) : IUseCaseInput
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

    if (Page < 1)
    {
      errors.Add("Page must be greater than zero.");
    }

    if (PageSize is < 1 or > 100)
    {
      errors.Add("PageSize must be between 1 and 100.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
