using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.ApprovalRoles.UpdateApprovalRole;

public sealed class UpdateApprovalRoleUseCase(
  IApprovalRoleRepository repository,
  ILogger<UpdateApprovalRoleUseCase> logger) : IUpdateApprovalRoleUseCase
{
  public async Task<Result<UpdateApprovalRoleUseCaseOutput>> HandleAsync(
    UpdateApprovalRoleUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[UpdateApprovalRoleUseCase][HandleAsync] Updating an approval role.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<UpdateApprovalRoleUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var profileIds = input.ProfileIds.Distinct().ToArray();
      var role = await repository.UpdateAsync(
        input.AccountIdentifier,
        input.AuthorityId,
        input.RoleId,
        input.ValueLimit,
        input.IsUnlimitedValueLimit,
        input.MinApprovers,
        profileIds,
        input.UpdatedBy,
        cancellationToken);

      if (role is null)
      {
        return Result<UpdateApprovalRoleUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<UpdateApprovalRoleUseCaseOutput>.Ok(
        new(
          role.Id,
          role.ValueLimit,
          role.IsUnlimitedValueLimit,
          role.MinApprovers,
          profileIds));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[UpdateApprovalRoleUseCase][HandleAsync] Approval role update failed.");

      return UseCaseFailure.From<UpdateApprovalRoleUseCaseOutput>(exception);
    }
  }
}
