namespace AccessManagement.Domain.Entities;

public sealed class Permission
{
  public Guid Id { get; set; }
  public string Description { get; set; } = string.Empty;
}
