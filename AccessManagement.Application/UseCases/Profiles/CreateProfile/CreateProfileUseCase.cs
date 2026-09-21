using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Profiles.CreateProfile;

public sealed class CreateProfileUseCase(
  IProfileRepository repository,
  ProfileFactory factory,
  ILogger<CreateProfileUseCase> logger) : ICreateProfileUseCase
{
  public async Task<Result<ProfileOutput>> HandleAsync(
    CreateProfileUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[CreateProfileUseCase][HandleAsync] Creating an account profile.");

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
      var profile = factory.Create(
        input.AccountIdentifier,
        input.Name,
        input.Description,
        input.CreatedBy);

      var created = await repository.InsertAsync(
        input.AccountIdentifier,
        profile,
        input.PermissionIds.Distinct().ToArray(),
        cancellationToken);

      if (created is null)
      {
        return Result<ProfileOutput>.Fail(
          "The account was not found.",
          ResultErrorType.NotFound);
      }

      return Result<ProfileOutput>.Ok(ProfileOutput.From(created));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[CreateProfileUseCase][HandleAsync] Profile creation failed.");

      return UseCaseFailure.From<ProfileOutput>(exception);
    }
  }
}
