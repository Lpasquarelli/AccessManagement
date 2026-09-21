using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class AccountFactory
{
  public Account Create(string identifier, Guid contextId, string holderIdentifier) =>
    new()
    {
      Identifier = identifier.Trim(),
      ContextId = contextId,
      HolderIdentifier = holderIdentifier.Trim(),
      Active = true
    };
}
