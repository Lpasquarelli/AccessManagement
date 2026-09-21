using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.Core.Repositories;

public interface IAccessCatalogRepository
{
  Task<AccessCatalog> GetAsync(CancellationToken cancellationToken = default);
}
