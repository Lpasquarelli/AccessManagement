using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Entities;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.InsertUser;

public sealed class InsertUserUseCase(
  IUserRepository userRepository,
  IUserFactory userFactory,
  ILogger<InsertUserUseCase> logger) : IInsertUserUseCase
{
  public async Task<Result<InsertUserUseCaseOutput>> HandleAsync(
    InsertUserUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[InsertUserUseCase][HandleAsync] Starting user creation.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      logger.LogWarning(
        "[InsertUserUseCase][HandleAsync] User creation input is invalid. ErrorCount: {ErrorCount}.",
        validation.Errors.Length);

      return Result<InsertUserUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var user = userFactory.Create(
        input.Name,
        input.Email,
        input.Phone,
        input.TaxId,
        input.AuthenticationId,
        input.IsBrazilResident,
        input.CreatedBy);

      var createdUser = await userRepository.InsertAsync(user, cancellationToken);

      logger.LogInformation(
        "[InsertUserUseCase][HandleAsync] User creation completed.");

      return Result<InsertUserUseCaseOutput>.Ok(MapOutput(createdUser));
    }
    catch (DataStoreUnavailableException exception)
    {
      logger.LogError(
        exception,
        "[InsertUserUseCase][HandleAsync] User creation failed because the data store is unavailable.");

      return Result<InsertUserUseCaseOutput>.Fail(
        "The data service is temporarily unavailable.",
        ResultErrorType.Unavailable);
    }
  }

  private static InsertUserUseCaseOutput MapOutput(User user) =>
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
