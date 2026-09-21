namespace AccessManagement.Domain.Entities;

public sealed class ProfilePermission
{
  public Guid Id { get; set; }
  public Guid ProfileId { get; set; }
  public Profile Profile { get; set; } = null!;
  public Guid PermissionId { get; set; }
}
