namespace AccessManagement.Domain.Entities;

public sealed class AccessContext
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public string Currency { get; set; } = string.Empty;
}
