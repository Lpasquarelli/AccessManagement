using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class AuthorityApprovalRoleFactory
{
  public AuthorityApprovalRole Create(
    Guid authorityId,
    decimal? valueLimit,
    bool isUnlimitedValueLimit,
    short minApprovers,
    string? createdBy)
  {
    if (minApprovers <= 0)
    {
      throw new ArgumentOutOfRangeException(nameof(minApprovers), "MinApprovers must be greater than zero.");
    }

    if (isUnlimitedValueLimit && valueLimit is not null)
    {
      throw new ArgumentException("ValueLimit must be null for an unlimited approval role.", nameof(valueLimit));
    }

    if (!isUnlimitedValueLimit && valueLimit is null or <= 0)
    {
      throw new ArgumentOutOfRangeException(nameof(valueLimit), "ValueLimit must be greater than zero.");
    }

    return new AuthorityApprovalRole
    {
      AuthorityId = authorityId,
      ValueLimit = valueLimit,
      IsUnlimitedValueLimit = isUnlimitedValueLimit,
      MinApprovers = minApprovers,
      Active = true,
      CreatedBy = FactoryValueNormalizer.Optional(createdBy)
    };
  }
}
