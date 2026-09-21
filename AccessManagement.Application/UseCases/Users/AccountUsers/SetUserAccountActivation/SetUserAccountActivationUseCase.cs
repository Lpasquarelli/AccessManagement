using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountActivation;

public sealed class SetUserAccountActivationUseCase(
  IUserAccountRepository repository,
  ILogger<SetUserAccountActivationUseCase> logger) : ISetUserAccountActivationUseCase
{
  public async Task<Result<SetUserAccountActivationUseCaseOutput>> HandleAsync(
    SetUserAccountActivationUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[SetUserAccountActivationUseCase][HandleAsync] Changing account user activation.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<SetUserAccountActivationUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var found = await repository.SetActivationAsync(
        input.AccountIdentifier,
        input.UserAccountId,
        input.Active,
        cancellationToken);

      if (!found)
      {
        return Result<SetUserAccountActivationUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<SetUserAccountActivationUseCaseOutput>.Ok(
        new(input.UserAccountId, input.Active));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[SetUserAccountActivationUseCase][HandleAsync] Account user activation change failed.");

      return UseCaseFailure.From<SetUserAccountActivationUseCaseOutput>(exception);
    }
  }
}
