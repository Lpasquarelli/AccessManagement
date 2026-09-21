using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Profiles.SetProfilePermissions;

public sealed class SetProfilePermissionsUseCase(
  IProfileRepository repository,
  ILogger<SetProfilePermissionsUseCase> logger) : ISetProfilePermissionsUseCase
{
  public async Task<Result<SetProfilePermissionsUseCaseOutput>> HandleAsync(
    SetProfilePermissionsUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[SetProfilePermissionsUseCase][HandleAsync] Replacing profile permissions.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<SetProfilePermissionsUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var permissionIds = input.PermissionIds.Distinct().ToArray();
      var found = await repository.ReplacePermissionsAsync(
        input.AccountIdentifier,
        input.ProfileId,
        permissionIds,
        cancellationToken);

      if (!found)
      {
        return Result<SetProfilePermissionsUseCaseOutput>.Fail(
          "The requested resource was not found.",
          ResultErrorType.NotFound);
      }

      return Result<SetProfilePermissionsUseCaseOutput>.Ok(
        new(input.ProfileId, permissionIds));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[SetProfilePermissionsUseCase][HandleAsync] Permission replacement failed.");

      return UseCaseFailure.From<SetProfilePermissionsUseCaseOutput>(exception);
    }
  }
}
