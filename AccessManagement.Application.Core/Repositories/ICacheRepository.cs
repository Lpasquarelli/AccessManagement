namespace AccessManagement.Application.Core.Repositories;

public interface ICacheRepository
{
  Task<T?> GetAsync<T>(
    string key,
    CancellationToken cancellationToken = default)
    where T : class;

  Task<T?> GetFallbackAsync<T>(
    string key,
    CancellationToken cancellationToken = default)
    where T : class;

  Task SetAsync<T>(
    string key,
    T value,
    int? ttlMinutes = null,
    CancellationToken cancellationToken = default)
    where T : class;

  Task SetFallbackAsync<T>(
    string key,
    T value,
    int? ttlMinutes = null,
    CancellationToken cancellationToken = default)
    where T : class;

  Task RemoveAsync(
    string key,
    CancellationToken cancellationToken = default);
}
