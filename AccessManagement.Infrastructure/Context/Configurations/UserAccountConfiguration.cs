using AccessManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessManagement.Infrastructure.Context.Configurations;

public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
  public void Configure(EntityTypeBuilder<UserAccount> builder)
  {
    builder.ToTable("UserAccount", "dbo");
    builder.HasKey(userAccount => userAccount.Id).HasName("PK_UserAccount");
    builder.Property(userAccount => userAccount.Id)
      .HasDefaultValueSql("NEWSEQUENTIALID()")
      .ValueGeneratedOnAdd();
    builder.Property(userAccount => userAccount.Active).HasDefaultValue(true).IsRequired();
    builder.Property(userAccount => userAccount.IsMaster).HasDefaultValue(false).IsRequired();
    builder.Property(userAccount => userAccount.IsHolder).HasDefaultValue(false).IsRequired();

    builder.HasAlternateKey(userAccount => new { userAccount.UserId, userAccount.AccountId })
      .HasName("UQ_UserAccount_UserId_AccountId");
    builder.HasIndex(userAccount => new { userAccount.AccountId, userAccount.Active })
      .HasDatabaseName("IX_UserAccount_AccountId_Active");

    builder.HasOne(userAccount => userAccount.User)
      .WithMany()
      .HasForeignKey(userAccount => userAccount.UserId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_UserAccount_User");
    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(userAccount => userAccount.AccountId)
      .OnDelete(DeleteBehavior.Restrict)
      .HasConstraintName("FK_UserAccount_Account");
  }
}
