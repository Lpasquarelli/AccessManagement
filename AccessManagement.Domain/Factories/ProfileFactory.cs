using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class ProfileFactory
{
  public Profile Create(
    string accountIdentifier,
    string name,
    string? description,
    string? createdBy) =>
    new()
    {
      AccountId = Guid.Empty,
      Name = name.Trim(),
      Description = FactoryValueNormalizer.Optional(description),
      CreatedBy = FactoryValueNormalizer.Optional(createdBy),
      Active = true
    };
}
