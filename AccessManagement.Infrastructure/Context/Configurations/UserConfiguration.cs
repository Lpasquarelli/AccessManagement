using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("User", "dbo");

    builder.HasKey(user => user.Id)
      .HasName("PK_User");

    builder.Property(user => user.Id)
      .HasColumnName("Id")
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();

    builder.Property(user => user.Name)
      .HasColumnName("Name")
      .HasMaxLength(150)
      .IsRequired();

    builder.Property(user => user.Email)
      .HasColumnName("Email")
      .HasMaxLength(320);

    builder.Property(user => user.Phone)
      .HasColumnName("Phone")
      .HasMaxLength(32);

    builder.Property(user => user.TaxId)
      .HasColumnName("TaxId")
      .HasMaxLength(20)
      .IsRequired();

    builder.Property(user => user.AuthenticationId)
      .HasColumnName("AuthenticationId")
      .HasMaxLength(150)
      .IsRequired();

    builder.Property(user => user.IsBrazilResident)
      .HasColumnName("IsBrazilResident")
      .HasDefaultValue(false)
      .IsRequired();

    builder.Property(user => user.Active)
      .HasColumnName("Active")
      .HasDefaultValue(true)
      .IsRequired();

    builder.Property(user => user.CreatedAt)
      .HasColumnName("CreatedAt")
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();

    builder.Property(user => user.UpdatedAt)
      .HasColumnName("UpdatedAt")
      .HasColumnType("datetime2(7)");

    builder.Property(user => user.CreatedBy)
      .HasColumnName("CreatedBy")
      .HasMaxLength(150);

    builder.Property(user => user.UpdatedBy)
      .HasColumnName("UpdatedBy")
      .HasMaxLength(150);

    builder.HasIndex(user => user.Email)
      .HasDatabaseName("IX_User_Email");

    builder.HasIndex(user => user.TaxId)
      .HasDatabaseName("IX_User_TaxId");

    builder.HasIndex(user => user.AuthenticationId)
      .IsUnique()
      .HasDatabaseName("UX_User_AuthenticationId");
  }
}
