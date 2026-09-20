using System.ComponentModel.DataAnnotations;

namespace AccessManagement.Infrastructure.Config;

public sealed class RedisOptions
{
  public const string SectionName = "Redis";

  [Required]
  public string ConnectionString { get; init; } = string.Empty;

  [Required]
  public string InstanceName { get; init; } = "AccessManagement";

  [Range(1, 1440)]
  public int FreshTtlMinutes { get; init; } = 10;

  [Range(1, 720)]
  public int FallbackTtlMinutes { get; init; } = 24;
}
