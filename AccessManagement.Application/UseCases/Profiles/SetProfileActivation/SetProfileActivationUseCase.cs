using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Profiles.SetProfileActivation;

public sealed class SetProfileActivationUseCase(
  IProfileRepository repository,
  ILogger<SetProfileActivationUseCase> logger) : ISetProfileActivationUseCase
{
  public async Task<Result<SetProfileActivationUseCaseOutput>> HandleAsync(
    SetProfileActivationUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[SetProfileActivationUseCase][HandleAsync] Changing profile activation.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<SetProfileActivationUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var found = await repository.SetActivationAsync(
        input.AccountIdentifier,
        input.ProfileId,
        input.Active,
        input.UpdatedBy,
        cancellationToken);

      if (!found)
      {
        return Result<SetProfileActivationUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<SetProfileActivationUseCaseOutput>.Ok(
        new(input.ProfileId, input.Active));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[SetProfileActivationUseCase][HandleAsync] Profile activation change failed.");

      return UseCaseFailure.From<SetProfileActivationUseCaseOutput>(exception);
    }
  }
}
