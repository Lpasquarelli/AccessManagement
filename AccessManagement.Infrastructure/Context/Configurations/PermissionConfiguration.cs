using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
  public void Configure(EntityTypeBuilder<Permission> builder)
  {
    builder.ToTable("Permission", "dbo");
    builder.HasKey(permission => permission.Id).HasName("PK_Permission");
    builder.Property(permission => permission.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(permission => permission.Description).HasMaxLength(500).IsRequired();

    builder.HasData(AccessCatalogSeed.Permissions);
  }
}
