using System.ComponentModel.DataAnnotations;

namespace AccessManagement.Infrastructure.Config;

public sealed class DatabaseOptions
{
  public const string SectionName = "Database";

  [Required]
  public string ConnectionString { get; init; } = string.Empty;

  [Range(1, 300)]
  public int CommandTimeoutSeconds { get; init; } = 30;

  [Range(0, 10)]
  public int MaxRetryCount { get; init; } = 3;

  [Range(1, 120)]
  public int MaxRetryDelaySeconds { get; init; } = 10;

  public bool ApplyMigrationsOnStartup { get; init; }
}
