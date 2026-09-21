using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
  public void Configure(EntityTypeBuilder<Account> builder)
  {
    builder.ToTable("Account", "dbo");
    builder.HasKey(account => account.Id).HasName("PK_Account");

    builder.Property(account => account.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(account => account.Identifier).HasMaxLength(50).IsRequired();
    builder.Property(account => account.HolderIdentifier).HasMaxLength(32).IsRequired();
    builder.Property(account => account.Active).HasDefaultValue(true).IsRequired();
    builder.Property(account => account.CreatedAt)
      .HasColumnType("datetime2(7)")
      .HasDefaultValueSql("SYSUTCDATETIME()")
      .ValueGeneratedOnAdd();

    builder.HasIndex(account => account.Identifier)
      .IsUnique()
      .HasDatabaseName("UQ_Account_Identifier");
    builder.HasIndex(account => account.ContextId)
      .HasDatabaseName("IX_Account_ContextId");

    builder.HasOne<AccessContext>()
      .WithMany()
      .HasForeignKey(account => account.ContextId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_Account_Context");
  }
}
