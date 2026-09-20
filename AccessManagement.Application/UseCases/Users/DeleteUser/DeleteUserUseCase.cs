using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.DeleteUser;

public sealed class DeleteUserUseCase(
  IUserRepository userRepository,
  ICacheRepository cacheRepository,
  IUserFactory userFactory,
  ILogger<DeleteUserUseCase> logger) : IDeleteUserUseCase
{
  public async Task<Result<DeleteUserUseCaseOutput>> HandleAsync(
    DeleteUserUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[DeleteUserUseCase][HandleAsync] Starting logical user deletion.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      logger.LogWarning(
        "[DeleteUserUseCase][HandleAsync] User deletion input is invalid. ErrorCount: {ErrorCount}.",
        validation.Errors.Length);

      return Result<DeleteUserUseCaseOutput>.Fail(
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
          "[DeleteUserUseCase][HandleAsync] User was not found.");

        return Result<DeleteUserUseCaseOutput>.Fail(
          "User was not found.");
      }

      userFactory.Deactivate(user, input.UpdatedBy);
      var deletedUser = await userRepository.UpdateAsync(user, cancellationToken);
      await cacheRepository.RemoveAsync(BuildCacheKey(input.Id), cancellationToken);

      logger.LogInformation(
        "[DeleteUserUseCase][HandleAsync] Logical user deletion completed.");

      return Result<DeleteUserUseCaseOutput>.Ok(new DeleteUserUseCaseOutput(
        deletedUser.Id,
        deletedUser.Active,
        deletedUser.UpdatedAt,
        deletedUser.UpdatedBy));
    }
    catch (DataStoreUnavailableException exception)
    {
      logger.LogError(
        exception,
        "[DeleteUserUseCase][HandleAsync] User deletion failed because the data store is unavailable.");

      return Result<DeleteUserUseCaseOutput>.Fail(
        "The data service is temporarily unavailable.",
        ResultErrorType.Unavailable);
    }
  }

  private static string BuildCacheKey(Guid id) => $"users:id:{id:N}";
}
