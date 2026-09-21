using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class AuthorityApprovalRoleConfiguration : IEntityTypeConfiguration<AuthorityApprovalRole>
{
  public void Configure(EntityTypeBuilder<AuthorityApprovalRole> builder)
  {
    builder.ToTable("AuthorityApprovalRole", "dbo", tableBuilder =>
    {
      tableBuilder.HasCheckConstraint(
        "CK_AuthorityApprovalRole_MinApprovers",
        "[MinApprovers] > 0");
      tableBuilder.HasCheckConstraint(
        "CK_AuthorityApprovalRole_ValueLimit",
        "([IsUnlimitedValueLimit] = 1 AND [ValueLimit] IS NULL) OR " +
        "([IsUnlimitedValueLimit] = 0 AND [ValueLimit] > 0)");
    });

    builder.HasKey(role => role.Id).HasName("PK_AuthorityApprovalRole");
    builder.Property(role => role.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(role => role.ValueLimit).HasColumnType("decimal(19,4)");
    builder.Property(role => role.IsUnlimitedValueLimit).HasDefaultValue(false).IsRequired();
    builder.Property(role => role.MinApprovers).HasColumnType("smallint").IsRequired();
    builder.Property(role => role.Active).HasDefaultValue(true).IsRequired();
    builder.Property(role => role.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();
    builder.Property(role => role.UpdatedAt).HasColumnType("datetime2(7)");
    builder.Property(role => role.CreatedBy).HasMaxLength(150);
    builder.Property(role => role.UpdatedBy).HasMaxLength(150);

    builder.HasIndex(role => new { role.AuthorityId, role.Active, role.ValueLimit })
      .HasDatabaseName("IX_AuthorityApprovalRole_AuthorityId_Active_ValueLimit");
    builder.HasIndex(role => new { role.AuthorityId, role.ValueLimit })
      .IsUnique()
      .HasFilter("[Active] = 1 AND [IsUnlimitedValueLimit] = 0")
      .HasDatabaseName("UX_AuthorityApprovalRole_Active_LimitedValue");
    builder.HasIndex(role => role.AuthorityId)
      .IsUnique()
      .HasFilter("[Active] = 1 AND [IsUnlimitedValueLimit] = 1")
      .HasDatabaseName("UX_AuthorityApprovalRole_Active_Unlimited");

    builder.HasOne<Authority>()
      .WithMany()
      .HasForeignKey(role => role.AuthorityId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_AuthorityApprovalRole_Authority");
  }
}
