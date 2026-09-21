using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class MakeAccountIdentifierUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_Account_ContextId_Identifier",
                schema: "dbo",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "UQ_Account_Identifier",
                schema: "dbo",
                table: "Account",
                column: "Identifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Account_Identifier",
                schema: "dbo",
                table: "Account");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_Account_ContextId_Identifier",
                schema: "dbo",
                table: "Account",
                columns: new[] { "ContextId", "Identifier" });
        }
    }
}
