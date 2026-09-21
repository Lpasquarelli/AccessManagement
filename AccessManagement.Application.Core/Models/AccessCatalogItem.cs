namespace AccessManagement.Application.Core.Models;

public sealed record CatalogPermission(
  Guid Id,
  string Description);

public sealed record CatalogPermissionGroup(
  string Name,
  bool HasApprovalPrivileges,
  IReadOnlyCollection<CatalogPermission> Permissions);

public sealed record AccessContextCatalogItem(
  Guid Id,
  string Name,
  string? Description,
  string Currency,
  IReadOnlyCollection<CatalogPermissionGroup> PermissionGroups);

public sealed record AuthorityCatalogItem(
  Guid Id,
  string Description);

public sealed record AccessCatalog(
  IReadOnlyCollection<AccessContextCatalogItem> Contexts,
  IReadOnlyCollection<AuthorityCatalogItem> Authorities);
