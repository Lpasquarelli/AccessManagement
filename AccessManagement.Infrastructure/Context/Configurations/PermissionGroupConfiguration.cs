using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
  public void Configure(EntityTypeBuilder<PermissionGroup> builder)
  {
    builder.ToTable("PermissionGroup", "dbo");
    builder.HasKey(group => group.Id).HasName("PK_PermissionGroup");
    builder.Property(group => group.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(group => group.Description).HasMaxLength(500).IsRequired();

    builder.HasAlternateKey(group => new { group.ContextId, group.PermissionId })
      .HasName("UQ_PermissionGroup_ContextId_PermissionId");
    builder.HasIndex(group => group.PermissionId)
      .HasDatabaseName("IX_PermissionGroup_PermissionId");

    builder.HasOne<Permission>()
      .WithMany()
      .HasForeignKey(group => group.PermissionId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_PermissionGroup_Permission");
    builder.HasOne<AccessContext>()
      .WithMany()
      .HasForeignKey(group => group.ContextId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_PermissionGroup_Context");

    builder.HasData(AccessCatalogSeed.PermissionGroups);
  }
}
