using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class ApprovalRoleProfileConfiguration : IEntityTypeConfiguration<ApprovalRoleProfile>
{
  public void Configure(EntityTypeBuilder<ApprovalRoleProfile> builder)
  {
    builder.ToTable("ApprovalRoleProfile", "dbo");
    builder.HasKey(roleProfile => roleProfile.Id).HasName("PK_ApprovalRoleProfile");
    builder.Property(roleProfile => roleProfile.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(roleProfile => roleProfile.Active).HasDefaultValue(true).IsRequired();
    builder.Property(roleProfile => roleProfile.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();
    builder.Property(roleProfile => roleProfile.CreatedBy).HasMaxLength(150);

    builder.HasAlternateKey(roleProfile => new
    {
      roleProfile.AuthorityApprovalRoleId,
      roleProfile.ProfileId
    })
      .HasName("UQ_ApprovalRoleProfile_AuthorityApprovalRoleId_ProfileId");
    builder.HasIndex(roleProfile => new
    {
      roleProfile.AuthorityApprovalRoleId,
      roleProfile.Active
    })
      .HasDatabaseName("IX_ApprovalRoleProfile_AuthorityApprovalRoleId_Active");
    builder.HasIndex(roleProfile => roleProfile.ProfileId)
      .HasDatabaseName("IX_ApprovalRoleProfile_ProfileId");

    builder.HasOne(roleProfile => roleProfile.AuthorityApprovalRole)
      .WithMany()
      .HasForeignKey(roleProfile => roleProfile.AuthorityApprovalRoleId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_ApprovalRoleProfile_AuthorityApprovalRole");
    builder.HasOne<Profile>()
      .WithMany()
      .HasForeignKey(roleProfile => roleProfile.ProfileId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_ApprovalRoleProfile_Profile");
  }
}
