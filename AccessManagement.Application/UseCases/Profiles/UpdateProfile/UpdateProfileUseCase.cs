using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Profiles.UpdateProfile;

public sealed class UpdateProfileUseCase(
  IProfileRepository repository,
  ILogger<UpdateProfileUseCase> logger) : IUpdateProfileUseCase
{
  public async Task<Result<ProfileOutput>> HandleAsync(
    UpdateProfileUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[UpdateProfileUseCase][HandleAsync] Updating an account profile.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<ProfileOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var profile = await repository.UpdateAsync(
        input.AccountIdentifier,
        input.ProfileId,
        input.Name,
        input.Description,
        input.UpdatedBy,
        cancellationToken);

      if (profile is null)
      {
        return Result<ProfileOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<ProfileOutput>.Ok(ProfileOutput.From(profile));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[UpdateProfileUseCase][HandleAsync] Profile update failed.");

      return UseCaseFailure.From<ProfileOutput>(exception);
    }
  }
}
