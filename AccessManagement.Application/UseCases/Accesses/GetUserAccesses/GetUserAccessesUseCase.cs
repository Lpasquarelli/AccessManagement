using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Accesses.GetUserAccesses;

public sealed class GetUserAccessesUseCase(
  IAccessRepository repository,
  ILogger<GetUserAccessesUseCase> logger) : IGetUserAccessesUseCase
{
  public async Task<Result<UserAccessSnapshot>> HandleAsync(
    GetUserAccessesUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[GetUserAccessesUseCase][HandleAsync] Querying effective access by authentication identifier.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<UserAccessSnapshot>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var access = await repository.GetByAuthenticationIdAsync(
        input.AuthenticationId.Trim(),
        cancellationToken);

      if (access is null)
      {
        return Result<UserAccessSnapshot>.Fail(
          "The active user was not found.",
          ResultErrorType.NotFound);
      }

      return Result<UserAccessSnapshot>.Ok(access);
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[GetUserAccessesUseCase][HandleAsync] Effective access query failed.");

      return UseCaseFailure.From<UserAccessSnapshot>(exception);
    }
  }
}
