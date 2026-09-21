using AccessManagement.Domain.Authorization;
using AccessManagement.Domain.Entities;

namespace AccessManagement.Infrastructure.Context;

internal static class AccessCatalogSeed
{
  internal static readonly AccessContext[] Contexts =
  [
    new()
    {
      Id = AccessContextCatalog.Onshore,
      Name = "ONSHORE",
      Description = "Contas nacionais em reais.",
      Currency = "BRL"
    },
    new()
    {
      Id = AccessContextCatalog.Offshore,
      Name = "OFFSHORE",
      Description = "Contas internacionais em dólares americanos.",
      Currency = "USD"
    }
  ];

  internal static readonly Permission[] Permissions =
  [
    Permission(PermissionCatalog.BalanceAndStatementRead, "Saldo / Extrato"),
    Permission(PermissionCatalog.BoletoRead, "Boletos"),
    Permission(PermissionCatalog.InvestmentsRead, "Investimentos"),
    Permission(PermissionCatalog.LinkedAccountRead, "Conta Vinculada"),
    Permission(PermissionCatalog.BeneficiaryApprove, "Favorecido"),
    Permission(PermissionCatalog.PaymentsApprove, "Pagamentos"),
    Permission(PermissionCatalog.SameOwnershipTransferApprove, "Transferências de mesma titularidade"),
    Permission(PermissionCatalog.OtherOwnershipTransferApprove, "Transferências de outras titularidades"),
    Permission(PermissionCatalog.PixApprove, "PIX"),
    Permission(PermissionCatalog.BatchPaymentsApprove, "Pagamentos em lote"),
    Permission(PermissionCatalog.BoletoCreate, "Boleto"),
    Permission(PermissionCatalog.PaymentsCreate, "Pagamentos"),
    Permission(PermissionCatalog.SameOwnershipTransferCreate, "Transferências de mesma titularidade"),
    Permission(PermissionCatalog.OtherOwnershipTransferCreate, "Transferências de outras titularidades"),
    Permission(PermissionCatalog.PixCreate, "PIX"),
    Permission(PermissionCatalog.BeneficiaryCreate, "Adicionar Favorecidos"),
    Permission(PermissionCatalog.BatchPaymentsCreate, "Pagamentos em lote"),
    Permission(PermissionCatalog.InvestmentsApply, "Aplicar em investimentos"),
    Permission(PermissionCatalog.ProfileManage, "Gestão de Perfis"),
    Permission(PermissionCatalog.AccessManage, "Gestão de Acessos"),
    Permission(PermissionCatalog.ApprovalPolicyManage, "Gestão de Alçadas"),
    Permission(PermissionCatalog.BoletoInstructions, "Instruções de Boletos"),
    Permission(PermissionCatalog.InvestmentsRedeem, "Resgatar Investimentos"),
    Permission(PermissionCatalog.LinkedAccountTransfer, "Transferência na conta vinculada"),
    Permission(PermissionCatalog.ReceivablesAdvance, "Antecipação de recebíveis"),
    Permission(PermissionCatalog.ForeignExchange, "Câmbio"),
    Permission(PermissionCatalog.Cards, "Cartões")
  ];

  internal static readonly PermissionGroup[] PermissionGroups =
  [
    Group(1, AccessContextCatalog.Onshore, PermissionCatalog.BalanceAndStatementRead, "Permissões de Consulta"),
    Group(2, AccessContextCatalog.Onshore, PermissionCatalog.BoletoRead, "Permissões de Consulta"),
    Group(3, AccessContextCatalog.Onshore, PermissionCatalog.InvestmentsRead, "Permissões de Consulta"),
    Group(4, AccessContextCatalog.Onshore, PermissionCatalog.LinkedAccountRead, "Permissões de Consulta"),
    Group(5, AccessContextCatalog.Onshore, PermissionCatalog.BeneficiaryApprove, "Permissões de Aprovação"),
    Group(6, AccessContextCatalog.Onshore, PermissionCatalog.PaymentsApprove, "Permissões de Aprovação"),
    Group(
      7,
      AccessContextCatalog.Onshore,
      PermissionCatalog.SameOwnershipTransferApprove,
      "Permissões de Aprovação"),
    Group(
      8,
      AccessContextCatalog.Onshore,
      PermissionCatalog.OtherOwnershipTransferApprove,
      "Permissões de Aprovação"),
    Group(9, AccessContextCatalog.Onshore, PermissionCatalog.PixApprove, "Permissões de Aprovação"),
    Group(10, AccessContextCatalog.Onshore, PermissionCatalog.BatchPaymentsApprove, "Permissões de Aprovação"),
    Group(11, AccessContextCatalog.Onshore, PermissionCatalog.BoletoCreate, "Permissões de Inclusão"),
    Group(12, AccessContextCatalog.Onshore, PermissionCatalog.PaymentsCreate, "Permissões de Inclusão"),
    Group(13, AccessContextCatalog.Onshore, PermissionCatalog.SameOwnershipTransferCreate, "Permissões de Inclusão"),
    Group(14, AccessContextCatalog.Onshore, PermissionCatalog.OtherOwnershipTransferCreate, "Permissões de Inclusão"),
    Group(15, AccessContextCatalog.Onshore, PermissionCatalog.PixCreate, "Permissões de Inclusão"),
    Group(16, AccessContextCatalog.Onshore, PermissionCatalog.BeneficiaryCreate, "Permissões de Inclusão"),
    Group(17, AccessContextCatalog.Onshore, PermissionCatalog.BatchPaymentsCreate, "Permissões de Inclusão"),
    Group(18, AccessContextCatalog.Onshore, PermissionCatalog.InvestmentsApply, "Outras Permissões"),
    Group(19, AccessContextCatalog.Onshore, PermissionCatalog.ProfileManage, "Outras Permissões"),
    Group(20, AccessContextCatalog.Onshore, PermissionCatalog.AccessManage, "Outras Permissões"),
    Group(21, AccessContextCatalog.Onshore, PermissionCatalog.ApprovalPolicyManage, "Outras Permissões"),
    Group(22, AccessContextCatalog.Onshore, PermissionCatalog.BoletoInstructions, "Outras Permissões"),
    Group(23, AccessContextCatalog.Onshore, PermissionCatalog.InvestmentsRedeem, "Outras Permissões"),
    Group(24, AccessContextCatalog.Onshore, PermissionCatalog.LinkedAccountTransfer, "Outras Permissões"),
    Group(25, AccessContextCatalog.Onshore, PermissionCatalog.ReceivablesAdvance, "Outras Permissões"),
    Group(26, AccessContextCatalog.Onshore, PermissionCatalog.ForeignExchange, "Outras Permissões"),
    Group(27, AccessContextCatalog.Onshore, PermissionCatalog.Cards, "Outras Permissões"),
    Group(28, AccessContextCatalog.Offshore, PermissionCatalog.PaymentsApprove, "Permissões de Aprovação"),
    Group(29, AccessContextCatalog.Offshore, PermissionCatalog.PaymentsCreate, "Permissões de Inclusão")
  ];

  private static Permission Permission(Guid id, string description) =>
    new()
    {
      Id = id,
      Description = description
    };

  private static PermissionGroup Group(
    int sequence,
    Guid contextId,
    Guid permissionId,
    string description) =>
    new()
    {
      Id = Guid.Parse($"30000000-0000-0000-0000-{sequence:000000000000}"),
      ContextId = contextId,
      PermissionId = permissionId,
      Description = description
    };
}
