using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.AccountUsers.AddUserToAccount;

public sealed class AddUserToAccountUseCase(
  IUserAccountRepository repository,
  IUserFactory userFactory,
  ILogger<AddUserToAccountUseCase> logger) : IAddUserToAccountUseCase
{
  public async Task<Result<AddUserToAccountUseCaseOutput>> HandleAsync(
    AddUserToAccountUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[AddUserToAccountUseCase][HandleAsync] Creating an operator for an account.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<AddUserToAccountUseCaseOutput>.Fail(
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

      var userAccount = await repository.CreateAsync(
        input.AccountIdentifier,
        user,
        input.ProfileIds.Distinct().ToArray(),
        input.CreatedBy,
        cancellationToken);

      if (userAccount is null)
      {
        return Result<AddUserToAccountUseCaseOutput>.Fail(
          "The account was not found.",
          ResultErrorType.NotFound);
      }

      return Result<AddUserToAccountUseCaseOutput>.Ok(
        new(
          userAccount.Id,
          userAccount.UserId,
          input.AccountIdentifier,
          userAccount.Active,
          userAccount.IsMaster,
          userAccount.IsHolder));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[AddUserToAccountUseCase][HandleAsync] Account user creation failed.");

      return UseCaseFailure.From<AddUserToAccountUseCaseOutput>(exception);
    }
  }
}
