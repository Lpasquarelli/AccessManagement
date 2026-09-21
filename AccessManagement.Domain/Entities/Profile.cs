namespace AccessManagement.Domain.Entities;

public sealed class Profile
{
  public Guid Id { get; set; }
  public Guid AccountId { get; set; }
  public string Name { get; set; } = string.Empty;
  public bool Active { get; set; }
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}
