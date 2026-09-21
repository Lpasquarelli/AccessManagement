using AccessManagement.Application.Core.Models;
using AccessManagement.Domain.Entities;

namespace AccessManagement.Application.Core.Repositories;

public interface IApprovalRoleRepository
{
  Task<IReadOnlyCollection<ApprovalRoleItem>> GetByAccountAsync(
    string accountIdentifier,
    Guid authorityId,
    CancellationToken cancellationToken = default);

  Task<AuthorityApprovalRole?> InsertAsync(
    string accountIdentifier,
    AuthorityApprovalRole role,
    IReadOnlyCollection<Guid> profileIds,
    CancellationToken cancellationToken = default);

  Task<AuthorityApprovalRole?> UpdateAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    decimal? valueLimit,
    bool unlimited,
    short minApprovers,
    IReadOnlyCollection<Guid> profileIds,
    string? updatedBy,
    CancellationToken cancellationToken = default);

  Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    bool active,
    bool confirmPrivilegeElevation,
    string? updatedBy,
    CancellationToken cancellationToken = default);
}
