using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.GetUserById;

public sealed class GetUserByIdUseCase(
  IUserRepository userRepository,
  ICacheRepository cacheRepository,
  ILogger<GetUserByIdUseCase> logger) : IGetUserByIdUseCase
{
  public async Task<Result<GetUserByIdUseCaseOutput>> HandleAsync(
    GetUserByIdUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[GetUserByIdUseCase][HandleAsync] Starting user lookup.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      logger.LogWarning(
        "[GetUserByIdUseCase][HandleAsync] User lookup input is invalid. ErrorCount: {ErrorCount}.",
        validation.Errors.Length);

      return Result<GetUserByIdUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    var cacheKey = $"users:id:{input.Id:N}";
    var cachedUser = await cacheRepository.GetAsync<User>(cacheKey, cancellationToken);
    if (cachedUser is not null)
    {
      logger.LogInformation(
        "[GetUserByIdUseCase][HandleAsync] User found in cache.");

      return Result<GetUserByIdUseCaseOutput>.Ok(MapOutput(cachedUser));
    }

    try
    {
      logger.LogInformation(
        "[GetUserByIdUseCase][HandleAsync] User not found in cache. Querying SQL Server.");

      var user = await userRepository.GetByIdAsync(input.Id, cancellationToken);
      if (user is null)
      {
        logger.LogInformation(
          "[GetUserByIdUseCase][HandleAsync] User was not found.");

        return Result<GetUserByIdUseCaseOutput>.Fail(
          "User was not found.",
          ResultErrorType.NotFound);
      }

      await cacheRepository.SetAsync(
        cacheKey,
        user,
        cancellationToken: cancellationToken);
      await cacheRepository.SetFallbackAsync(
        cacheKey,
        user,
        cancellationToken: cancellationToken);

      logger.LogInformation(
        "[GetUserByIdUseCase][HandleAsync] User lookup completed and caches were updated.");

      return Result<GetUserByIdUseCaseOutput>.Ok(MapOutput(user));
    }
    catch (DataStoreUnavailableException exception)
    {
      logger.LogWarning(
        exception,
        "[GetUserByIdUseCase][HandleAsync] SQL Server is unavailable. Querying the fallback cache.");

      var fallbackUser = await cacheRepository.GetFallbackAsync<User>(cacheKey, cancellationToken);
      if (fallbackUser is not null)
      {
        logger.LogWarning(
          "[GetUserByIdUseCase][HandleAsync] Returning a user from the fallback cache.");

        return Result<GetUserByIdUseCaseOutput>.Ok(MapOutput(fallbackUser));
      }

      logger.LogError(
        "[GetUserByIdUseCase][HandleAsync] SQL Server is unavailable and no fallback value exists.");

      return Result<GetUserByIdUseCaseOutput>.Fail(
        "The data service is temporarily unavailable.",
        ResultErrorType.Unavailable);
    }
  }

  private static GetUserByIdUseCaseOutput MapOutput(User user) =>
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
