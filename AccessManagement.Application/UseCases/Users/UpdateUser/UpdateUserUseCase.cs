using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Entities;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.UpdateUser;

public sealed class UpdateUserUseCase(
  IUserRepository userRepository,
  ICacheRepository cacheRepository,
  IUserFactory userFactory,
  ILogger<UpdateUserUseCase> logger) : IUpdateUserUseCase
{
  public async Task<Result<UpdateUserUseCaseOutput>> HandleAsync(
    UpdateUserUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[UpdateUserUseCase][HandleAsync] Starting user update.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      logger.LogWarning(
        "[UpdateUserUseCase][HandleAsync] User update input is invalid. ErrorCount: {ErrorCount}.",
        validation.Errors.Length);

      return Result<UpdateUserUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var user = await userRepository.GetByIdAsync(input.Id, cancellationToken);
      if (user is null)
      {
        logger.LogInformation(
          "[UpdateUserUseCase][HandleAsync] User was not found.");

        return Result<UpdateUserUseCaseOutput>.Fail(
          "User was not found.",
          ResultErrorType.NotFound);
      }

      userFactory.Update(
        user,
        input.Name,
        input.Email,
        input.Phone,
        input.TaxId,
        input.AuthenticationId,
        input.IsBrazilResident,
        input.UpdatedBy);

      var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);
      await cacheRepository.RemoveAsync(BuildCacheKey(input.Id), cancellationToken);

      logger.LogInformation(
        "[UpdateUserUseCase][HandleAsync] User update completed.");

      return Result<UpdateUserUseCaseOutput>.Ok(MapOutput(updatedUser));
    }
    catch (DataStoreUnavailableException exception)
    {
      logger.LogError(
        exception,
        "[UpdateUserUseCase][HandleAsync] User update failed because the data store is unavailable.");

      return Result<UpdateUserUseCaseOutput>.Fail(
        "The data service is temporarily unavailable.",
        ResultErrorType.Unavailable);
    }
  }

  private static string BuildCacheKey(Guid id) => $"users:id:{id:N}";

  private static UpdateUserUseCaseOutput MapOutput(User user) =>
    new(
      user.Id,
      user.Name,
      user.Email,
      user.Phone,
      user.TaxId,
      user.AuthenticationId,
      user.IsBrazilResident,
      user.Active,
      user.CreatedAt,
      user.UpdatedAt,
      user.CreatedBy,
      user.UpdatedBy);
}
