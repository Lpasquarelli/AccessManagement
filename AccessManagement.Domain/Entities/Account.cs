namespace AccessManagement.Domain.Entities;

public sealed class Account
{
  public Guid Id { get; set; }
  public string Identifier { get; set; } = string.Empty;
  public Guid ContextId { get; set; }
  public string HolderIdentifier { get; set; } = string.Empty;
  public bool Active { get; set; }
  public DateTime CreatedAt { get; set; }
}
