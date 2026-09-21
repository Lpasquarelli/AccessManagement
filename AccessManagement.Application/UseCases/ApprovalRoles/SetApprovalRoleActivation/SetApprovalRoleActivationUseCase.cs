using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.ApprovalRoles.SetApprovalRoleActivation;

public sealed class SetApprovalRoleActivationUseCase(
  IApprovalRoleRepository repository,
  ILogger<SetApprovalRoleActivationUseCase> logger) : ISetApprovalRoleActivationUseCase
{
  public async Task<Result<SetApprovalRoleActivationUseCaseOutput>> HandleAsync(
    SetApprovalRoleActivationUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[SetApprovalRoleActivationUseCase][HandleAsync] Changing approval role activation.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<SetApprovalRoleActivationUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var found = await repository.SetActivationAsync(
        input.AccountIdentifier,
        input.AuthorityId,
        input.RoleId,
        input.Active,
        input.ConfirmPrivilegeElevation,
        input.UpdatedBy,
        cancellationToken);

      if (!found)
      {
        return Result<SetApprovalRoleActivationUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<SetApprovalRoleActivationUseCaseOutput>.Ok(
        new(input.RoleId, input.Active));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[SetApprovalRoleActivationUseCase][HandleAsync] Approval role activation change failed.");

      return UseCaseFailure.From<SetApprovalRoleActivationUseCaseOutput>(exception);
    }
  }
}
