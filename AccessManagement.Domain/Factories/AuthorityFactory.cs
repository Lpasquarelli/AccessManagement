using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class AuthorityFactory
{
  public Authority Create(string description) =>
    new()
    {
      Description = description.Trim()
    };
}
