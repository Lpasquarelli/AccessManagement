using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Core.Models;
using AccessManagement.Application.Core.Repositories;
using AccessManagement.Domain.Authorization;
using AccessManagement.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Infrastructure.Repositories;

public sealed class AccessCatalogRepository(
  AccessManagementDbContext dbContext,
  ILogger<AccessCatalogRepository> logger) : IAccessCatalogRepository
{
  public async Task<AccessCatalog> GetAsync(CancellationToken cancellationToken = default)
  {
    logger.LogDebug(
      "[AccessCatalogRepository][GetAsync] Querying access catalog.");

    try
    {
      var contexts = await dbContext.AccessContexts
        .AsNoTracking()
        .OrderBy(item => item.Name)
        .ToArrayAsync(cancellationToken);

      var groupRows = await (
        from permissionGroup in dbContext.PermissionGroups.AsNoTracking()
        join permission in dbContext.Permissions.AsNoTracking()
          on permissionGroup.PermissionId equals permission.Id
        select new
        {
          Group = permissionGroup,
          Permission = permission
        }).ToArrayAsync(cancellationToken);

      var authorities = await dbContext.Authorities
        .AsNoTracking()
        .OrderBy(item => item.Description)
        .Select(item => new AuthorityCatalogItem(item.Id, item.Description))
        .ToArrayAsync(cancellationToken);

      var contextItems = contexts
        .Select(context => new AccessContextCatalogItem(
          context.Id,
          context.Name,
          context.Description,
          context.Currency,
          groupRows
            .Where(item => item.Group.ContextId == context.Id)
            .GroupBy(item => item.Group.Description)
            .Select(group => new CatalogPermissionGroup(
              group.Key,
              group.Any(item => PermissionCatalog.ApprovalPermissions.Contains(item.Permission.Id)),
              group
                .Select(item => new CatalogPermission(
                  item.Permission.Id,
                  item.Permission.Description))
                .OrderBy(item => item.Description)
                .ToArray()))
            .OrderBy(item => item.Name)
            .ToArray()))
        .ToArray();

      return new AccessCatalog(contextItems, authorities);
    }
    catch (SqlException exception)
    {
      logger.LogError(
        exception,
        "[AccessCatalogRepository][GetAsync] SQL Server communication failed while querying the access catalog.");

      throw new DataStoreUnavailableException("SQL Server is unavailable.", exception);
    }
  }
}
