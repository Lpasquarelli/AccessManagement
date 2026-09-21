using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Application.UseCases.Users.AccountUsers;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Users.AccountUsers.ListAccountUsers;

public sealed class ListAccountUsersUseCase(
  IUserAccountRepository repository,
  ILogger<ListAccountUsersUseCase> logger) : IListAccountUsersUseCase
{
  public async Task<Result<IReadOnlyCollection<AccountUserOutput>>> HandleAsync(
    ListAccountUsersUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[ListAccountUsersUseCase][HandleAsync] Listing account users.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<IReadOnlyCollection<AccountUserOutput>>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var users = await repository.GetByAccountAsync(
        input.AccountIdentifier,
        input.Page,
        input.PageSize,
        cancellationToken);

      return Result<IReadOnlyCollection<AccountUserOutput>>.Ok(
        users.Select(AccountUserOutput.From).ToArray());
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[ListAccountUsersUseCase][HandleAsync] Account user listing failed.");

      return UseCaseFailure.From<IReadOnlyCollection<AccountUserOutput>>(exception);
    }
  }
}
