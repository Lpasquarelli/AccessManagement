using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class UserAccountProfileConfiguration : IEntityTypeConfiguration<UserAccountProfile>
{
  public void Configure(EntityTypeBuilder<UserAccountProfile> builder)
  {
    builder.ToTable("UserAccountProfiles", "dbo");
    builder.HasKey(assignment => assignment.Id).HasName("PK_UserAccountProfiles");
    builder.Property(assignment => assignment.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(assignment => assignment.Active).HasDefaultValue(true).IsRequired();
    builder.Property(assignment => assignment.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();
    builder.Property(assignment => assignment.UpdatedAt).HasColumnType("datetime2(7)");
    builder.Property(assignment => assignment.CreatedBy).HasMaxLength(150);
    builder.Property(assignment => assignment.UpdatedBy).HasMaxLength(150);

    builder.HasAlternateKey(assignment => new { assignment.UserAccountId, assignment.ProfileId })
      .HasName("UQ_UserAccountProfiles_UserAccountId_ProfileId");
    builder.HasIndex(assignment => new { assignment.UserAccountId, assignment.Active })
      .HasDatabaseName("IX_UserAccountProfiles_UserAccountId_Active");
    builder.HasIndex(assignment => assignment.ProfileId)
      .HasDatabaseName("IX_UserAccountProfiles_ProfileId");

    builder.HasOne(assignment => assignment.UserAccount)
      .WithMany()
      .HasForeignKey(assignment => assignment.UserAccountId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_UserAccountProfiles_UserAccount");
    builder.HasOne<Profile>()
      .WithMany()
      .HasForeignKey(assignment => assignment.ProfileId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_UserAccountProfiles_Profile");
  }
}
