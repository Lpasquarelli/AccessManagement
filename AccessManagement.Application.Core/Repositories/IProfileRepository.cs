using AccessManagement.Application.Core.Models;
using AccessManagement.Domain.Entities;

namespace AccessManagement.Application.Core.Repositories;

public interface IProfileRepository
{
  Task<IReadOnlyCollection<ProfileItem>> GetByAccountAsync(
    string accountIdentifier,
    CancellationToken cancellationToken = default);

  Task<ProfileItem?> InsertAsync(
    string accountIdentifier,
    Profile profile,
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken = default);

  Task<ProfileItem?> UpdateAsync(
    string accountIdentifier,
    Guid profileId,
    string name,
    string? description,
    string? updatedBy,
    CancellationToken cancellationToken = default);

  Task<bool> SetActivationAsync(
    string accountIdentifier,
    Guid profileId,
    bool active,
    string? updatedBy,
    CancellationToken cancellationToken = default);

  Task<bool> ReplacePermissionsAsync(
    string accountIdentifier,
    Guid profileId,
    IReadOnlyCollection<Guid> permissionIds,
    CancellationToken cancellationToken = default);
}
