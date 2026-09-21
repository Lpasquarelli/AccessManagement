using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.Core.Repositories;

public interface IAccessRepository
{
  Task<UserAccessSnapshot?> GetByAuthenticationIdAsync(
    string authenticationId,
    CancellationToken cancellationToken = default);

  Task<AccountAccessEvaluation?> EvaluateAsync(
    string authenticationId,
    string accountIdentifier,
    Guid permissionId,
    Guid authorityId,
    decimal? amount,
    CancellationToken cancellationToken = default);
}
