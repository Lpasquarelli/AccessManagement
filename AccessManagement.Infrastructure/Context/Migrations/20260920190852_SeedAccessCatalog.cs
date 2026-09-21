using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccessManagement.Infrastructure.Context.Migrations
{
  /// <inheritdoc />
  public partial class SeedAccessCatalog : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
          schema: "dbo",
          table: "Context",
          columns: new[] { "Id", "Currency", "Description", "Name" },
          values: new object[,]
          {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "BRL", "Contas nacionais em reais.", "ONSHORE" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "USD", "Contas internacionais em dólares americanos.", "OFFSHORE" }
          });

      migrationBuilder.InsertData(
          schema: "dbo",
          table: "Permission",
          columns: new[] { "Id", "Description" },
          values: new object[,]
          {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Saldo / Extrato" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Boletos" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Investimentos" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Conta Vinculada" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Favorecido" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Pagamentos" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Transferências de mesma titularidade" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Transferências de outras titularidades" },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "PIX" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Pagamentos em lote" },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "Boleto" },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "Pagamentos" },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "Transferências de mesma titularidade" },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "Transferências de outras titularidades" },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "PIX" },
                    { new Guid("10000000-0000-0000-0000-000000000016"), "Adicionar Favorecidos" },
                    { new Guid("10000000-0000-0000-0000-000000000017"), "Pagamentos em lote" },
                    { new Guid("10000000-0000-0000-0000-000000000018"), "Aplicar em investimentos" },
                    { new Guid("10000000-0000-0000-0000-000000000019"), "Gestão de Perfis" },
                    { new Guid("10000000-0000-0000-0000-000000000020"), "Gestão de Acessos" },
                    { new Guid("10000000-0000-0000-0000-000000000021"), "Gestão de Alçadas" },
                    { new Guid("10000000-0000-0000-0000-000000000022"), "Instruções de Boletos" },
                    { new Guid("10000000-0000-0000-0000-000000000023"), "Resgatar Investimentos" },
                    { new Guid("10000000-0000-0000-0000-000000000024"), "Transferência na conta vinculada" },
                    { new Guid("10000000-0000-0000-0000-000000000025"), "Antecipação de recebíveis" },
                    { new Guid("10000000-0000-0000-0000-000000000026"), "Câmbio" },
                    { new Guid("10000000-0000-0000-0000-000000000027"), "Cartões" }
          });

      migrationBuilder.InsertData(
          schema: "dbo",
          table: "PermissionGroup",
          columns: new[] { "Id", "ContextId", "Description", "PermissionId" },
          values: new object[,]
          {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Consulta", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Consulta", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Consulta", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Consulta", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("30000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("30000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("30000000-0000-0000-0000-000000000010"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000010") },
                    { new Guid("30000000-0000-0000-0000-000000000011"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000011") },
                    { new Guid("30000000-0000-0000-0000-000000000012"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000012") },
                    { new Guid("30000000-0000-0000-0000-000000000013"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000013") },
                    { new Guid("30000000-0000-0000-0000-000000000014"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000014") },
                    { new Guid("30000000-0000-0000-0000-000000000015"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000015") },
                    { new Guid("30000000-0000-0000-0000-000000000016"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000016") },
                    { new Guid("30000000-0000-0000-0000-000000000017"), new Guid("20000000-0000-0000-0000-000000000001"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000017") },
                    { new Guid("30000000-0000-0000-0000-000000000018"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000018") },
                    { new Guid("30000000-0000-0000-0000-000000000019"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000019") },
                    { new Guid("30000000-0000-0000-0000-000000000020"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000020") },
                    { new Guid("30000000-0000-0000-0000-000000000021"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000021") },
                    { new Guid("30000000-0000-0000-0000-000000000022"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000022") },
                    { new Guid("30000000-0000-0000-0000-000000000023"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000023") },
                    { new Guid("30000000-0000-0000-0000-000000000024"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000024") },
                    { new Guid("30000000-0000-0000-0000-000000000025"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000025") },
                    { new Guid("30000000-0000-0000-0000-000000000026"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000026") },
                    { new Guid("30000000-0000-0000-0000-000000000027"), new Guid("20000000-0000-0000-0000-000000000001"), "Outras Permissões", new Guid("10000000-0000-0000-0000-000000000027") },
                    { new Guid("30000000-0000-0000-0000-000000000028"), new Guid("20000000-0000-0000-0000-000000000002"), "Permissões de Aprovação", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000029"), new Guid("20000000-0000-0000-0000-000000000002"), "Permissões de Inclusão", new Guid("10000000-0000-0000-0000-000000000012") }
          });

    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000006"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000007"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000008"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000009"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000010"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000011"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000012"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000013"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000014"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000015"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000016"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000017"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000018"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000019"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000020"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000021"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000022"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000023"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000024"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000025"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000026"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000027"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000028"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "PermissionGroup",
          keyColumn: "Id",
          keyValue: new Guid("30000000-0000-0000-0000-000000000029"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Context",
          keyColumn: "Id",
          keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Context",
          keyColumn: "Id",
          keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000011"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000012"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000013"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000014"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000015"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000016"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000017"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000018"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000019"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000020"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000021"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000022"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000023"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000024"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000025"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000026"));

      migrationBuilder.DeleteData(
          schema: "dbo",
          table: "Permission",
          keyColumn: "Id",
          keyValue: new Guid("10000000-0000-0000-0000-000000000027"));

    }
  }
}
