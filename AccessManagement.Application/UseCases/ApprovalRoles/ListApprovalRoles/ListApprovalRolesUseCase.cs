using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.ApprovalRoles.ListApprovalRoles;

public sealed class ListApprovalRolesUseCase(
  IApprovalRoleRepository repository,
  ILogger<ListApprovalRolesUseCase> logger) : IListApprovalRolesUseCase
{
  public async Task<Result<IReadOnlyCollection<ApprovalRoleOutput>>> HandleAsync(
    ListApprovalRolesUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[ListApprovalRolesUseCase][HandleAsync] Listing approval roles.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<IReadOnlyCollection<ApprovalRoleOutput>>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var roles = await repository.GetByAccountAsync(
        input.AccountIdentifier,
        input.AuthorityId,
        cancellationToken);

      return Result<IReadOnlyCollection<ApprovalRoleOutput>>.Ok(
        roles.Select(ApprovalRoleOutput.From).ToArray());
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[ListApprovalRolesUseCase][HandleAsync] Approval role listing failed.");

      return UseCaseFailure.From<IReadOnlyCollection<ApprovalRoleOutput>>(exception);
    }
  }
}
