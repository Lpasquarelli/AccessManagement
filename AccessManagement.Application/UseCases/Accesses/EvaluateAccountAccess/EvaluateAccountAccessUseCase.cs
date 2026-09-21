using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Accesses.EvaluateAccountAccess;

public sealed class EvaluateAccountAccessUseCase(
  IAccessRepository repository,
  ILogger<EvaluateAccountAccessUseCase> logger) : IEvaluateAccountAccessUseCase
{
  public async Task<Result<AccountAccessEvaluation>> HandleAsync(
    EvaluateAccountAccessUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[EvaluateAccountAccessUseCase][HandleAsync] Evaluating user rules for an account.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<AccountAccessEvaluation>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var evaluation = await repository.EvaluateAsync(
        input.AuthenticationId.Trim(),
        input.AccountIdentifier,
        input.PermissionId,
        input.AuthorityId,
        input.Amount,
        cancellationToken);

      if (evaluation is null)
      {
        return Result<AccountAccessEvaluation>.Fail(
          "The active user or account access was not found.",
          ResultErrorType.NotFound);
      }

      return Result<AccountAccessEvaluation>.Ok(evaluation);
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[EvaluateAccountAccessUseCase][HandleAsync] Account access evaluation failed.");

      return UseCaseFailure.From<AccountAccessEvaluation>(exception);
    }
  }
}
