using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountProfiles;

public sealed class SetUserAccountProfilesUseCase(
  IUserAccountRepository repository,
  ILogger<SetUserAccountProfilesUseCase> logger) : ISetUserAccountProfilesUseCase
{
  public async Task<Result<SetUserAccountProfilesUseCaseOutput>> HandleAsync(
    SetUserAccountProfilesUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[SetUserAccountProfilesUseCase][HandleAsync] Replacing account user profiles.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<SetUserAccountProfilesUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var profileIds = input.ProfileIds.Distinct().ToArray();
      var found = await repository.ReplaceProfilesAsync(
        input.AccountIdentifier,
        input.UserAccountId,
        profileIds,
        input.UpdatedBy,
        cancellationToken);

      if (!found)
      {
        return Result<SetUserAccountProfilesUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<SetUserAccountProfilesUseCaseOutput>.Ok(
        new(input.UserAccountId, profileIds));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[SetUserAccountProfilesUseCase][HandleAsync] Account user profile replacement failed.");

      return UseCaseFailure.From<SetUserAccountProfilesUseCaseOutput>(exception);
    }
  }
}
