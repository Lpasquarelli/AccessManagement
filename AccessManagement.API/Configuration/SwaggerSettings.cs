using System.ComponentModel.DataAnnotations;

namespace AccessManagement.API.Configuration;

public sealed class SwaggerSettings
{
  public const string SectionName = "Swagger";

  public bool Enabled { get; init; } = true;

  [Required]
  public string Title { get; init; } = "Access Management API";

  public string Description { get; init; } = "Access Management HTTP API";
}
