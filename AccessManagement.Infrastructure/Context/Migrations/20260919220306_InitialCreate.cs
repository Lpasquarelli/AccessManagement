using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Infrastructure.Context.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.EnsureSchema(
          name: "dbo");

      migrationBuilder.CreateTable(
          name: "User",
          schema: "dbo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()")
                  .Annotation("Relational:DefaultConstraintName", "DF_User_Id"),
            Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
            Phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
            TaxId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            AuthenticationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            IsBrazilResident = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                  .Annotation("Relational:DefaultConstraintName", "DF_User_IsBrazilResident"),
            Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                  .Annotation("Relational:DefaultConstraintName", "DF_User_Active"),
            CreatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                  .Annotation("Relational:DefaultConstraintName", "DF_User_CreatedAt"),
            UpdatedAt = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
            CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_User", x => x.Id);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "User",
          schema: "dbo");
    }
  }
}
