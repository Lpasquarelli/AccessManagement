using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessManagement.Infrastructure.Context;

public sealed class AccessManagementDbContext(DbContextOptions<AccessManagementDbContext> options)
  : DbContext(options)
{
  public DbSet<User> Users => Set<User>();
  public DbSet<AccessContext> AccessContexts => Set<AccessContext>();
  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<Permission> Permissions => Set<Permission>();
  public DbSet<Authority> Authorities => Set<Authority>();
  public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
  public DbSet<Profile> Profiles => Set<Profile>();
  public DbSet<PermissionGroup> PermissionGroups => Set<PermissionGroup>();
  public DbSet<UserAccountProfile> UserAccountProfiles => Set<UserAccountProfile>();
  public DbSet<ProfilePermission> ProfilePermissions => Set<ProfilePermission>();
  public DbSet<AuthorityApprovalRole> AuthorityApprovalRoles => Set<AuthorityApprovalRole>();
  public DbSet<ApprovalRoleProfile> ApprovalRoleProfiles => Set<ApprovalRoleProfile>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseNamedDefaultConstraints();
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccessManagementDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}
