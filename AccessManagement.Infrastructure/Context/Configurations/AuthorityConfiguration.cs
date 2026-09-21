using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class AuthorityConfiguration : IEntityTypeConfiguration<Authority>
{
  public void Configure(EntityTypeBuilder<Authority> builder)
  {
    builder.ToTable("Authority", "dbo");
    builder.HasKey(authority => authority.Id).HasName("PK_Authority");
    builder.Property(authority => authority.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(authority => authority.Description).HasMaxLength(500).IsRequired();
    builder.Property(authority => authority.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();
  }
}
