using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class AccessContextFactory
{
  public AccessContext Create(string name, string? description, string currency) =>
    new()
    {
      Name = name.Trim(),
      Description = FactoryValueNormalizer.Optional(description),
      Currency = currency.Trim().ToUpperInvariant()
    };
}
