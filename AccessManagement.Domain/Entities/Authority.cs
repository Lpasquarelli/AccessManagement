namespace AccessManagement.Domain.Entities;

public sealed class Authority
{
  public Guid Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}
