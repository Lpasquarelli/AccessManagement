namespace AccessManagement.Domain.Entities;

public sealed class ApprovalRoleProfile
{
  public Guid Id { get; set; }
  public Guid AuthorityApprovalRoleId { get; set; }
  public AuthorityApprovalRole AuthorityApprovalRole { get; set; } = null!;
  public Guid ProfileId { get; set; }
  public bool Active { get; set; }
  public DateTime CreatedAt { get; set; }
  public string? CreatedBy { get; set; }
}
