using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class PermissionGroupFactory
{
  public PermissionGroup Create(Guid permissionId, string description, Guid contextId) =>
    new()
    {
      PermissionId = permissionId,
      Description = description.Trim(),
      ContextId = contextId
    };
}
