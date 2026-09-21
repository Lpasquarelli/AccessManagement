using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class PermissionFactory
{
  public Permission Create(string description) =>
    new()
    {
      Description = description.Trim()
    };
}
