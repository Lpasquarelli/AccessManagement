namespace AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;

public sealed record CreateApprovalRoleUseCaseInput(
  string AccountIdentifier,
  Guid AuthorityId,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  Guid[] ProfileIds,
  string? CreatedBy) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = Validate(
      AccountIdentifier,
      AuthorityId,
      ValueLimit,
      IsUnlimitedValueLimit,
      MinApprovers,
      ProfileIds,
      CreatedBy);

    return (errors.Length == 0, errors);
  }

  internal static string[] Validate(
    string accountIdentifier,
    Guid authorityId,
    decimal? valueLimit,
    bool unlimited,
    short minApprovers,
    Guid[]? profileIds,
    string? actor)
  {
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(accountIdentifier))
    {
      errors.Add("AccountIdentifier is required.");
    }
    else if (accountIdentifier.Trim().Length > 50)
    {
      errors.Add("AccountIdentifier must contain at most 50 characters.");
    }

    if (authorityId == Guid.Empty)
    {
      errors.Add("AuthorityId is required.");
    }

    if (minApprovers <= 0)
    {
      errors.Add("MinApprovers must be greater than zero.");
    }

    if (unlimited && valueLimit is not null)
    {
      errors.Add("ValueLimit must be null for an unlimited role.");
    }

    if (!unlimited && valueLimit is null or <= 0)
    {
      errors.Add("ValueLimit must be greater than zero for a limited role.");
    }

    if (profileIds is null ||
        profileIds.Length == 0 ||
        profileIds.Any(id => id == Guid.Empty))
    {
      errors.Add("At least one valid ProfileId is required.");
    }

    if (actor?.Trim().Length > 150)
    {
      errors.Add("Actor identifier must contain at most 150 characters.");
    }

    return errors.ToArray();
  }
}
