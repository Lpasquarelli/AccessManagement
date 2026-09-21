namespace AccessManagement.Domain.Entities;

public sealed class PermissionGroup
{
  public Guid Id { get; set; }
  public Guid PermissionId { get; set; }
  public string Description { get; set; } = string.Empty;
  public Guid ContextId { get; set; }
}
