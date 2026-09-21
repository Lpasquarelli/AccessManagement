using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Infrastructure.Context.Migrations
{
  /// <inheritdoc />
  public partial class ChangeUpdatedByToString : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "UpdatedBy",
          schema: "dbo",
          table: "User",
          type: "nvarchar(150)",
          maxLength: 150,
          nullable: true,
          oldClrType: typeof(Guid),
          oldType: "uniqueidentifier",
          oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<Guid>(
          name: "UpdatedBy",
          schema: "dbo",
          table: "User",
          type: "uniqueidentifier",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(150)",
          oldMaxLength: 150,
          oldNullable: true);
    }
  }
}
