using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Infrastructure.Context.Migrations
{
  /// <inheritdoc />
  public partial class AddAuthorizationAndApprovalModel : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Authority",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Authority_Id"),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Authority_CreatedAt")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Authority", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Context",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Context_Id"),
            Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            Currency = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Context", x => x.Id);
            table.UniqueConstraint("UQ_Context_Name", x => x.Name);
          });

      migrationBuilder.CreateTable(
          name: "Permission",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Permission_Id"),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Permission", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "AuthorityApprovalRole",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_AuthorityApprovalRole_Id"),
            AuthorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            ValueLimit = table.Column<decimal>(type: "decimal(19,4)", nullable: true),
            IsUnlimitedValueLimit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                  .Annotation("Relational:DefaultConstraintName", "DF_AuthorityApprovalRole_IsUnlimitedValueLimit"),
            MinApprovers = table.Column<short>(type: "smallint", nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_AuthorityApprovalRole_Active"),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_AuthorityApprovalRole_CreatedAt"),
            UpdatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AuthorityApprovalRole", x => x.Id);
            table.CheckConstraint("CK_AuthorityApprovalRole_MinApprovers", "[MinApprovers] > 0");
            table.CheckConstraint("CK_AuthorityApprovalRole_ValueLimit", "([IsUnlimitedValueLimit] = 1 AND [ValueLimit] IS NULL) OR ([IsUnlimitedValueLimit] = 0 AND [ValueLimit] > 0)");
            table.ForeignKey(
                      name: "FK_AuthorityApprovalRole_Authority",
                      column: x => x.AuthorityId,
                      principalSchema: "dbo",
                      principalTable: "Authority",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "Account",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Account_Id"),
            Identifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            ContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            HolderIdentifier = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_Account_Active"),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Account_CreatedAt")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Account", x => x.Id);
            table.UniqueConstraint("UQ_Account_ContextId_Identifier", x => new { x.ContextId, x.Identifier });
            table.ForeignKey(
                      name: "FK_Account_Context",
                      column: x => x.ContextId,
                      principalSchema: "dbo",
                      principalTable: "Context",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "PermissionGroup",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_PermissionGroup_Id"),
            PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            ContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_PermissionGroup", x => x.Id);
            table.UniqueConstraint("UQ_PermissionGroup_ContextId_PermissionId", x => new { x.ContextId, x.PermissionId });
            table.ForeignKey(
                      name: "FK_PermissionGroup_Context",
                      column: x => x.ContextId,
                      principalSchema: "dbo",
                      principalTable: "Context",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_PermissionGroup_Permission",
                      column: x => x.PermissionId,
                      principalSchema: "dbo",
                      principalTable: "Permission",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "Profile",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Profile_Id"),
            AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_Profile_Active"),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_Profile_CreatedAt"),
            UpdatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Profile", x => x.Id);
            table.UniqueConstraint("UQ_Profile_AccountId_Name", x => new { x.AccountId, x.Name });
            table.ForeignKey(
                      name: "FK_Profile_Account",
                      column: x => x.AccountId,
                      principalSchema: "dbo",
                      principalTable: "Account",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "UserAccount",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccount_Id"),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccount_Active"),
            AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            IsMaster = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccount_IsMaster"),
            IsHolder = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccount_IsHolder")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserAccount", x => x.Id);
            table.UniqueConstraint("UQ_UserAccount_UserId_AccountId", x => new { x.UserId, x.AccountId });
            table.ForeignKey(
                      name: "FK_UserAccount_Account",
                      column: x => x.AccountId,
                      principalSchema: "dbo",
                      principalTable: "Account",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_UserAccount_User",
                      column: x => x.UserId,
                      principalSchema: "dbo",
                      principalTable: "User",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "ApprovalRoleProfile",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_ApprovalRoleProfile_Id"),
            AuthorityApprovalRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_ApprovalRoleProfile_Active"),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_ApprovalRoleProfile_CreatedAt"),
            CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ApprovalRoleProfile", x => x.Id);
            table.UniqueConstraint("UQ_ApprovalRoleProfile_AuthorityApprovalRoleId_ProfileId", x => new { x.AuthorityApprovalRoleId, x.ProfileId });
            table.ForeignKey(
                      name: "FK_ApprovalRoleProfile_AuthorityApprovalRole",
                      column: x => x.AuthorityApprovalRoleId,
                      principalSchema: "dbo",
                      principalTable: "AuthorityApprovalRole",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_ApprovalRoleProfile_Profile",
                      column: x => x.ProfileId,
                      principalSchema: "dbo",
                      principalTable: "Profile",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "ProfilePermission",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_ProfilePermission_Id"),
            ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ProfilePermission", x => x.Id);
            table.UniqueConstraint("UQ_ProfilePermission_ProfileId_PermissionId", x => new { x.ProfileId, x.PermissionId });
            table.ForeignKey(
                      name: "FK_ProfilePermission_Permission",
                      column: x => x.PermissionId,
                      principalSchema: "dbo",
                      principalTable: "Permission",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_ProfilePermission_Profile",
                      column: x => x.ProfileId,
                      principalSchema: "dbo",
                      principalTable: "Profile",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "UserAccountProfiles",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccountProfiles_Id"),
            UserAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccountProfiles_Active"),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_UserAccountProfiles_CreatedAt"),
            UpdatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserAccountProfiles", x => x.Id);
            table.UniqueConstraint("UQ_UserAccountProfiles_UserAccountId_ProfileId", x => new { x.UserAccountId, x.ProfileId });
            table.ForeignKey(
                      name: "FK_UserAccountProfiles_Profile",
                      column: x => x.ProfileId,
                      principalSchema: "dbo",
                      principalTable: "Profile",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_UserAccountProfiles_UserAccount",
                      column: x => x.UserAccountId,
                      principalSchema: "dbo",
                      principalTable: "UserAccount",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_User_AuthenticationId",
          schema: "dbo",
          table: "User",
          column: "AuthenticationId");

      migrationBuilder.CreateIndex(
          name: "IX_User_Email",
          schema: "dbo",
          table: "User",
          column: "Email");

      migrationBuilder.CreateIndex(
          name: "IX_User_TaxId",
          schema: "dbo",
          table: "User",
          column: "TaxId");

      migrationBuilder.CreateIndex(
          name: "IX_Account_ContextId",
          schema: "dbo",
          table: "Account",
          column: "ContextId");

      migrationBuilder.CreateIndex(
          name: "IX_ApprovalRoleProfile_AuthorityApprovalRoleId_Active",
          schema: "dbo",
          table: "ApprovalRoleProfile",
          columns: new[] { "AuthorityApprovalRoleId", "Active" });

      migrationBuilder.CreateIndex(
          name: "IX_ApprovalRoleProfile_ProfileId",
          schema: "dbo",
          table: "ApprovalRoleProfile",
          column: "ProfileId");

      migrationBuilder.CreateIndex(
          name: "IX_AuthorityApprovalRole_AuthorityId_Active_ValueLimit",
          schema: "dbo",
          table: "AuthorityApprovalRole",
          columns: new[] { "AuthorityId", "Active", "ValueLimit" });

      migrationBuilder.CreateIndex(
          name: "UX_AuthorityApprovalRole_Active_LimitedValue",
          schema: "dbo",
          table: "AuthorityApprovalRole",
          columns: new[] { "AuthorityId", "ValueLimit" },
          unique: true,
          filter: "[Active] = 1 AND [IsUnlimitedValueLimit] = 0");

      migrationBuilder.CreateIndex(
          name: "UX_AuthorityApprovalRole_Active_Unlimited",
          schema: "dbo",
          table: "AuthorityApprovalRole",
          column: "AuthorityId",
          unique: true,
          filter: "[Active] = 1 AND [IsUnlimitedValueLimit] = 1");

      migrationBuilder.CreateIndex(
          name: "IX_PermissionGroup_PermissionId",
          schema: "dbo",
          table: "PermissionGroup",
          column: "PermissionId");

      migrationBuilder.CreateIndex(
          name: "IX_Profile_AccountId_Active",
          schema: "dbo",
          table: "Profile",
          columns: new[] { "AccountId", "Active" });

      migrationBuilder.CreateIndex(
          name: "IX_ProfilePermission_PermissionId",
          schema: "dbo",
          table: "ProfilePermission",
          column: "PermissionId");

      migrationBuilder.CreateIndex(
          name: "IX_UserAccount_AccountId_Active",
          schema: "dbo",
          table: "UserAccount",
          columns: new[] { "AccountId", "Active" });

      migrationBuilder.CreateIndex(
          name: "IX_UserAccountProfiles_ProfileId",
          schema: "dbo",
          table: "UserAccountProfiles",
          column: "ProfileId");

      migrationBuilder.CreateIndex(
          name: "IX_UserAccountProfiles_UserAccountId_Active",
          schema: "dbo",
          table: "UserAccountProfiles",
          columns: new[] { "UserAccountId", "Active" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "ApprovalRoleProfile",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "PermissionGroup",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "ProfilePermission",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "UserAccountProfiles",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "AuthorityApprovalRole",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "Permission",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "Profile",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "UserAccount",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "Authority",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "Account",
          schema: "dbo");

      migrationBuilder.DropTable(
          name: "Context",
          schema: "dbo");

      migrationBuilder.DropIndex(
          name: "IX_User_AuthenticationId",
          schema: "dbo",
          table: "User");

      migrationBuilder.DropIndex(
          name: "IX_User_Email",
          schema: "dbo",
          table: "User");

      migrationBuilder.DropIndex(
          name: "IX_User_TaxId",
          schema: "dbo",
          table: "User");
    }
  }
}
