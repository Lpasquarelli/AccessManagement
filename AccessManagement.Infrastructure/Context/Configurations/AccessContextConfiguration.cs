using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class AccessContextConfiguration : IEntityTypeConfiguration<AccessContext>
{
  public void Configure(EntityTypeBuilder<AccessContext> builder)
  {
    builder.ToTable("Context", "dbo");
    builder.HasKey(context => context.Id).HasName("PK_Context");

    builder.Property(context => context.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(context => context.Name).HasMaxLength(150).IsRequired();
    builder.Property(context => context.Description).HasMaxLength(500);
    builder.Property(context => context.Currency)
      .HasColumnType("char(3)")
      .IsFixedLength()
      .HasMaxLength(3)
      .IsRequired();

    builder.HasAlternateKey(context => context.Name)
      .HasName("UQ_Context_Name");

    builder.HasData(AccessCatalogSeed.Contexts);
  }
}
