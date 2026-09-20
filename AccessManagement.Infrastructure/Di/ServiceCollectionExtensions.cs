using AccessManagement.Application.Core.Repositories;
using AccessManagement.Infrastructure.Config;
using AccessManagement.Infrastructure.Context;
using AccessManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AccessManagement.Infrastructure.Di;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    services.AddOptions<DatabaseOptions>()
      .Bind(configuration.GetSection(DatabaseOptions.SectionName))
      .ValidateDataAnnotations()
      .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
        "Database:ConnectionString is required.")
      .ValidateOnStart();

    services.AddOptions<RedisOptions>()
      .Bind(configuration.GetSection(RedisOptions.SectionName))
      .ValidateDataAnnotations()
      .Validate(
        options => options.FallbackTtlMinutes > options.FreshTtlMinutes,
        "Redis:FallbackTtlMinutes must be greater than Redis:FreshTtlMinutes.")
      .ValidateOnStart();

    services.AddDbContext<AccessManagementDbContext>((serviceProvider, optionsBuilder) =>
    {
      var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
      optionsBuilder.UseSqlServer(
        databaseOptions.ConnectionString,
        sqlServerOptions =>
        {
          sqlServerOptions.CommandTimeout(databaseOptions.CommandTimeoutSeconds);
          sqlServerOptions.EnableRetryOnFailure(
            databaseOptions.MaxRetryCount,
            TimeSpan.FromSeconds(databaseOptions.MaxRetryDelaySeconds),
            null);
        });
    });

    services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
    {
      var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
      var connectionOptions = ConfigurationOptions.Parse(redisOptions.ConnectionString);
      connectionOptions.AbortOnConnectFail = false;
      return ConnectionMultiplexer.Connect(connectionOptions);
    });

    services.AddSingleton<ICacheRepository, CacheRepository>();
    services.AddScoped<IUserRepository, UserRepository>();

    return services;
  }

  public static async Task ApplyDatabaseMigrationsAsync(
    this IServiceProvider services,
    CancellationToken cancellationToken = default)
  {
    await using var scope = services.CreateAsyncScope();
    var logger = scope.ServiceProvider
      .GetRequiredService<ILogger<AccessManagementDbContext>>();

    logger.LogInformation(
      "[ServiceCollectionExtensions][ApplyDatabaseMigrationsAsync] Applying database migrations.");

    var dbContext = scope.ServiceProvider.GetRequiredService<AccessManagementDbContext>();
    await dbContext.Database.MigrateAsync(cancellationToken);

    logger.LogInformation(
      "[ServiceCollectionExtensions][ApplyDatabaseMigrationsAsync] Database migrations were applied.");
  }
}
