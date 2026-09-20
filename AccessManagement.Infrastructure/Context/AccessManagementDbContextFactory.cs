using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccessManagement.Infrastructure.Context;

public sealed class AccessManagementDbContextFactory
  : IDesignTimeDbContextFactory<AccessManagementDbContext>
{
  public AccessManagementDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AccessManagementDbContext>();
    optionsBuilder.UseSqlServer(
      "Server=localhost,1433;Database=AccessManagement;User Id=sa;Password=DesignTimeOnly123!;TrustServerCertificate=True;Encrypt=True");

    return new AccessManagementDbContext(optionsBuilder.Options);
  }
}
