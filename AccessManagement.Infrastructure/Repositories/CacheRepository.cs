using System.Text.Json;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class CacheRepository(
  IConnectionMultiplexer connectionMultiplexer,
  IOptions<RedisOptions> options,
  ILogger<CacheRepository> logger) : ICacheRepository
{
  private readonly RedisOptions _options = options.Value;
  private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

  public Task<T?> GetAsync<T>(
    string key,
    CancellationToken cancellationToken = default)
    where T : class =>
    GetValueAsync<T>(BuildKey(key, "fresh"), cancellationToken);

  public Task<T?> GetFallbackAsync<T>(
    string key,
    CancellationToken cancellationToken = default)
    where T : class =>
    GetValueAsync<T>(BuildKey(key, "fallback"), cancellationToken);

  public Task SetAsync<T>(
    string key,
    T value,
    int? ttlMinutes = null,
    CancellationToken cancellationToken = default)
    where T : class =>
    SetValueAsync(
      BuildKey(key, "fresh"),
      value,
      GetExpiration(ttlMinutes, _options.FreshTtlMinutes),
      cancellationToken);

  public Task SetFallbackAsync<T>(
    string key,
    T value,
    int? ttlMinutes = null,
    CancellationToken cancellationToken = default)
    where T : class =>
    SetValueAsync(
      BuildKey(key, "fallback"),
      value,
      GetExpiration(ttlMinutes, _options.FallbackTtlMinutes),
      cancellationToken);

  public async Task RemoveAsync(
    string key,
    CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();

    try
    {
      await connectionMultiplexer.GetDatabase().KeyDeleteAsync([
        BuildKey(key, "fresh"),
        BuildKey(key, "fallback")
      ]);
    }
    catch (RedisException exception)
    {
      logger.LogWarning(
        exception,
        "[CacheRepository][RemoveAsync] Redis values could not be removed.");
    }
  }

  private async Task<T?> GetValueAsync<T>(
    RedisKey key,
    CancellationToken cancellationToken)
    where T : class
  {
    cancellationToken.ThrowIfCancellationRequested();

    try
    {
      var value = await connectionMultiplexer.GetDatabase().StringGetAsync(key);
      if (value.IsNullOrEmpty)
        return null;

      return JsonSerializer.Deserialize<T>((string)value!, _serializerOptions);
    }
    catch (Exception exception) when (exception is RedisException or JsonException)
    {
      logger.LogWarning(
        exception,
        "[CacheRepository][GetValueAsync] Redis value could not be read.");
      return null;
    }
  }

  private async Task SetValueAsync<T>(
    RedisKey key,
    T value,
    TimeSpan expiration,
    CancellationToken cancellationToken)
    where T : class
  {
    cancellationToken.ThrowIfCancellationRequested();

    try
    {
      var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
      await connectionMultiplexer
        .GetDatabase()
        .StringSetAsync(key, serializedValue, expiration);
    }
    catch (Exception exception) when (exception is RedisException or JsonException)
    {
      logger.LogWarning(
        exception,
        "[CacheRepository][SetValueAsync] Redis value could not be stored.");
    }
  }

  private RedisKey BuildKey(string key, string suffix) =>
    $"{_options.InstanceName}:{key}:{suffix}";

  private static TimeSpan GetExpiration(int? ttlMinutes, int defaultTtlMinutes)
  {
    var resolvedTtlMinutes = ttlMinutes ?? defaultTtlMinutes;
    if (resolvedTtlMinutes <= 0)
      throw new ArgumentOutOfRangeException(
        nameof(ttlMinutes),
        "TTL must be greater than zero minutes.");

    return TimeSpan.FromMinutes(resolvedTtlMinutes);
  }
}
