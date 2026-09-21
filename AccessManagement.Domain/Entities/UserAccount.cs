namespace AccessManagement.Domain.Entities;

public sealed class UserAccount
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public User User { get; set; } = null!;
  public bool Active { get; set; }
  public Guid AccountId { get; set; }
  public bool IsMaster { get; set; }
  public bool IsHolder { get; set; }
}
