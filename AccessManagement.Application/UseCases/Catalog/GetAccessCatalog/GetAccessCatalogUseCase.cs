using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.Catalog.GetAccessCatalog;

public sealed class GetAccessCatalogUseCase(
  IAccessCatalogRepository repository,
  ICacheRepository cacheRepository,
  ILogger<GetAccessCatalogUseCase> logger) : IGetAccessCatalogUseCase
{
  public async Task<Result<AccessCatalog>> HandleAsync(
    GetAccessCatalogUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    const string cacheKey = "access-catalog:v1";

    logger.LogInformation(
      "[GetAccessCatalogUseCase][HandleAsync] Querying the access catalog.");

    var cached = await cacheRepository.GetAsync<AccessCatalog>(cacheKey, cancellationToken);
    if (cached is not null)
    {
      return Result<AccessCatalog>.Ok(cached);
    }

    try
    {
      var catalog = await repository.GetAsync(cancellationToken);

      await cacheRepository.SetAsync(
        cacheKey,
        catalog,
        cancellationToken: cancellationToken);

      await cacheRepository.SetFallbackAsync(
        cacheKey,
        catalog,
        cancellationToken: cancellationToken);

      return Result<AccessCatalog>.Ok(catalog);
    }
    catch (DataStoreUnavailableException exception)
    {
      logger.LogError(
        exception,
        "[GetAccessCatalogUseCase][HandleAsync] SQL Server is unavailable while querying the catalog.");

      var fallback = await cacheRepository.GetFallbackAsync<AccessCatalog>(
        cacheKey,
        cancellationToken);

      return fallback is not null
        ? Result<AccessCatalog>.Ok(fallback)
        : Result<AccessCatalog>.Fail(
          "The access catalog is temporarily unavailable.",
          ResultErrorType.Unavailable);
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[GetAccessCatalogUseCase][HandleAsync] Access catalog query failed.");

      return UseCaseFailure.From<AccessCatalog>(exception);
    }
  }
}
