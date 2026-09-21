using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class ProfilePermissionConfiguration : IEntityTypeConfiguration<ProfilePermission>
{
  public void Configure(EntityTypeBuilder<ProfilePermission> builder)
  {
    builder.ToTable("ProfilePermission", "dbo");
    builder.HasKey(profilePermission => profilePermission.Id).HasName("PK_ProfilePermission");
    builder.Property(profilePermission => profilePermission.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();

    builder.HasAlternateKey(profilePermission => new { profilePermission.ProfileId, profilePermission.PermissionId })
      .HasName("UQ_ProfilePermission_ProfileId_PermissionId");
    builder.HasIndex(profilePermission => profilePermission.PermissionId)
      .HasDatabaseName("IX_ProfilePermission_PermissionId");

    builder.HasOne(profilePermission => profilePermission.Profile)
      .WithMany()
      .HasForeignKey(profilePermission => profilePermission.ProfileId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_ProfilePermission_Profile");
    builder.HasOne<Permission>()
      .WithMany()
      .HasForeignKey(profilePermission => profilePermission.PermissionId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_ProfilePermission_Permission");
  }
}
