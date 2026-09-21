namespace AccessManagement.Domain.Entities;

public sealed class AuthorityApprovalRole
{
  public Guid Id { get; set; }
  public Guid AuthorityId { get; set; }
  public decimal? ValueLimit { get; set; }
  public bool IsUnlimitedValueLimit { get; set; }
  public short MinApprovers { get; set; }
  public bool Active { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}
