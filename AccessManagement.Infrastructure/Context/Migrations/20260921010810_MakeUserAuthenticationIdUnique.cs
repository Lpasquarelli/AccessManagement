using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Infrastructure.Context.Migrations
{
  /// <inheritdoc />
  public partial class MakeUserAuthenticationIdUnique : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "IX_User_AuthenticationId",
          schema: "dbo",
          table: "User");

      migrationBuilder.CreateIndex(
          name: "UX_User_AuthenticationId",
          schema: "dbo",
          table: "User",
          column: "AuthenticationId",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "UX_User_AuthenticationId",
          schema: "dbo",
          table: "User");

      migrationBuilder.CreateIndex(
          name: "IX_User_AuthenticationId",
          schema: "dbo",
          table: "User",
          column: "AuthenticationId");
    }
  }
}
