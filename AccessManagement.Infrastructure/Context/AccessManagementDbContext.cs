using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessManagement.Infrastructure.Context;

public sealed class AccessManagementDbContext(DbContextOptions<AccessManagementDbContext> options)
  : DbContext(options)
{
  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseNamedDefaultConstraints();
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccessManagementDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}
