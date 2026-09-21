using AccessManagement.Application.Core.Models;
using AccessManagement.Domain.Entities;

namespace AccessManagement.Application.Core.Repositories;

public interface IUserAccountRepository
{
  Task<IReadOnlyCollection<AccountUserItem>> GetByAccountAsync(
    string accountIdentifier,
    int page,
    int pageSize,
    CancellationToken cancellationToken = default);

  Task<UserAccount?> CreateAsync(
    string accountIdentifier,
    User user,
    IReadOnlyCollection<Guid> profileIds,
    string? createdBy,
    CancellationToken cancellationToken = default);

  Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid userAccountId,
    bool active,
    CancellationToken cancellationToken = default);

  Task<bool> ReplaceProfilesAsync(
    string accountIdentifier,
    Guid userAccountId,
    IReadOnlyCollection<Guid> profileIds,
    string? updatedBy,
    CancellationToken cancellationToken = default);
}
