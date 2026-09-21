namespace AccessManagement.Domain.Authorization;

public static class PermissionCatalog
{
  public static readonly Guid BalanceAndStatementRead = new("10000000-0000-0000-0000-000000000001");
  public static readonly Guid BoletoRead = new("10000000-0000-0000-0000-000000000002");
  public static readonly Guid InvestmentsRead = new("10000000-0000-0000-0000-000000000003");
  public static readonly Guid LinkedAccountRead = new("10000000-0000-0000-0000-000000000004");
  public static readonly Guid BeneficiaryApprove = new("10000000-0000-0000-0000-000000000005");
  public static readonly Guid PaymentsApprove = new("10000000-0000-0000-0000-000000000006");
  public static readonly Guid SameOwnershipTransferApprove = new("10000000-0000-0000-0000-000000000007");
  public static readonly Guid OtherOwnershipTransferApprove = new("10000000-0000-0000-0000-000000000008");
  public static readonly Guid PixApprove = new("10000000-0000-0000-0000-000000000009");
  public static readonly Guid BatchPaymentsApprove = new("10000000-0000-0000-0000-000000000010");
  public static readonly Guid BoletoCreate = new("10000000-0000-0000-0000-000000000011");
  public static readonly Guid PaymentsCreate = new("10000000-0000-0000-0000-000000000012");
  public static readonly Guid SameOwnershipTransferCreate = new("10000000-0000-0000-0000-000000000013");
  public static readonly Guid OtherOwnershipTransferCreate = new("10000000-0000-0000-0000-000000000014");
  public static readonly Guid PixCreate = new("10000000-0000-0000-0000-000000000015");
  public static readonly Guid BeneficiaryCreate = new("10000000-0000-0000-0000-000000000016");
  public static readonly Guid BatchPaymentsCreate = new("10000000-0000-0000-0000-000000000017");
  public static readonly Guid InvestmentsApply = new("10000000-0000-0000-0000-000000000018");
  public static readonly Guid ProfileManage = new("10000000-0000-0000-0000-000000000019");
  public static readonly Guid AccessManage = new("10000000-0000-0000-0000-000000000020");
  public static readonly Guid ApprovalPolicyManage = new("10000000-0000-0000-0000-000000000021");
  public static readonly Guid BoletoInstructions = new("10000000-0000-0000-0000-000000000022");
  public static readonly Guid InvestmentsRedeem = new("10000000-0000-0000-0000-000000000023");
  public static readonly Guid LinkedAccountTransfer = new("10000000-0000-0000-0000-000000000024");
  public static readonly Guid ReceivablesAdvance = new("10000000-0000-0000-0000-000000000025");
  public static readonly Guid ForeignExchange = new("10000000-0000-0000-0000-000000000026");
  public static readonly Guid Cards = new("10000000-0000-0000-0000-000000000027");

  public static IReadOnlySet<Guid> ApprovalPermissions { get; } = new HashSet<Guid>
  {
    BeneficiaryApprove,
    PaymentsApprove,
    SameOwnershipTransferApprove,
    OtherOwnershipTransferApprove,
    PixApprove,
    BatchPaymentsApprove
  };
}

public static class AccessContextCatalog
{
  public static readonly Guid Onshore = new("20000000-0000-0000-0000-000000000001");
  public static readonly Guid Offshore = new("20000000-0000-0000-0000-000000000002");
}
