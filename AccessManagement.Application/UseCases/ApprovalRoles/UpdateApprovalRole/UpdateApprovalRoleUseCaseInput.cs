using AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;

namespace AccessManagement.Application.UseCases.ApprovalRoles.UpdateApprovalRole;

public sealed record UpdateApprovalRoleUseCaseInput(
  string AccountIdentifier,
  Guid AuthorityId,
  Guid RoleId,
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  Guid[] ProfileIds,
  string? UpdatedBy) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = CreateApprovalRoleUseCaseInput.Validate(
      AccountIdentifier,
      AuthorityId,
      ValueLimit,
      IsUnlimitedValueLimit,
      MinApprovers,
      ProfileIds,
      UpdatedBy).ToList();

    if (RoleId == Guid.Empty)
    {
      errors.Add("RoleId is required.");
    }

    return (errors.Count == 0, errors.ToArray());
  }
}
