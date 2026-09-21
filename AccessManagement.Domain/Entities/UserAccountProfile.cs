namespace AccessManagement.Domain.Entities;

public sealed class UserAccountProfile
{
  public Guid Id { get; set; }
  public Guid UserAccountId { get; set; }
  public UserAccount UserAccount { get; set; } = null!;
  public Guid ProfileId { get; set; }
  public bool Active { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}
