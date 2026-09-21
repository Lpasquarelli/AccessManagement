using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
  public void Configure(EntityTypeBuilder<Profile> builder)
  {
    builder.ToTable("Profile", "dbo");
    builder.HasKey(profile => profile.Id).HasName("PK_Profile");
    builder.Property(profile => profile.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(profile => profile.Name).HasMaxLength(150).IsRequired();
    builder.Property(profile => profile.Active).HasDefaultValue(true).IsRequired();
    builder.Property(profile => profile.Description).HasMaxLength(500);
    builder.Property(profile => profile.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();
    builder.Property(profile => profile.UpdatedAt).HasColumnType("datetime2(7)");
    builder.Property(profile => profile.CreatedBy).HasMaxLength(150);
    builder.Property(profile => profile.UpdatedBy).HasMaxLength(150);

    builder.HasAlternateKey(profile => new { profile.AccountId, profile.Name })
      .HasName("UQ_Profile_AccountId_Name");
    builder.HasIndex(profile => new { profile.AccountId, profile.Active })
      .HasDatabaseName("IX_Profile_AccountId_Active");

    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(profile => profile.AccountId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_Profile_Account");
  }
}
