namespace AccessManagement.Domain.Entities;

public sealed class User
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string TaxId { get; set; } = string.Empty;
  public string AuthenticationId { get; set; } = string.Empty;
  public bool IsBrazilResident { get; set; }
  public bool Active { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public string? CreatedBy { get; set; }
  public string? UpdatedBy { get; set; }
}
