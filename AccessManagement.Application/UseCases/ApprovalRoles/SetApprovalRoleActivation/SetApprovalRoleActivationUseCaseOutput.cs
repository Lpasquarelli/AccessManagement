namespace AccessManagement.Application.UseCases.ApprovalRoles.SetApprovalRoleActivation;

public sealed record SetApprovalRoleActivationUseCaseOutput(Guid RoleId, bool Active);
