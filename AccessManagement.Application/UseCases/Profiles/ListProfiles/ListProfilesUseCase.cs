using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Profiles.ListProfiles;

public sealed class ListProfilesUseCase(
  IProfileRepository repository,
  ILogger<ListProfilesUseCase> logger) : IListProfilesUseCase
{
  public async Task<Result<IReadOnlyCollection<ProfileOutput>>> HandleAsync(
    ListProfilesUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[ListProfilesUseCase][HandleAsync] Listing account profiles.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<IReadOnlyCollection<ProfileOutput>>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var profiles = await repository.GetByAccountAsync(
        input.AccountIdentifier,
        cancellationToken);

      return Result<IReadOnlyCollection<ProfileOutput>>.Ok(
        profiles.Select(ProfileOutput.From).ToArray());
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[ListProfilesUseCase][HandleAsync] Profile listing failed.");

      return UseCaseFailure.From<IReadOnlyCollection<ProfileOutput>>(exception);
    }
  }
}
