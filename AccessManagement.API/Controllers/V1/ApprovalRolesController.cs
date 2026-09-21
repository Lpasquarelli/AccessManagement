using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;
using AccessManagement.Application.UseCases.ApprovalRoles.ListApprovalRoles;
using AccessManagement.Application.UseCases.ApprovalRoles.SetApprovalRoleActivation;
using AccessManagement.Application.UseCases.ApprovalRoles.UpdateApprovalRole;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/accounts/{accountIdentifier}/authorities/{authorityId:guid}/approval-roles")]
public sealed class ApprovalRolesController(
  IListApprovalRolesUseCase listUseCase,
  ICreateApprovalRoleUseCase createUseCase,
  IUpdateApprovalRoleUseCase updateUseCase,
  ISetApprovalRoleActivationUseCase activationUseCase,
  ILogger<ApprovalRolesController> logger) : ControllerBase
{
  [EndpointSummary("Lista as faixas de alçada")]
  [EndpointDescription("Retorna diretamente as faixas ativas da autoridade para a conta informada.")]
  [HttpGet]
  public async Task<IActionResult> ListAsync(
    string accountIdentifier,
    Guid authorityId,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ApprovalRolesController][ListAsync] Received a request to list approval roles.");

    var result = await listUseCase.HandleAsync(
      new(accountIdentifier, authorityId),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Cria uma faixa de alçada")]
  [EndpointDescription("Cria uma regra de valor e associa os perfis aprovadores pertencentes à mesma conta.")]
  [HttpPost]
  public async Task<IActionResult> CreateAsync(
    string accountIdentifier,
    Guid authorityId,
    [FromBody] ApprovalRoleRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ApprovalRolesController][CreateAsync] Received a request to create an approval role.");

    var result = await createUseCase.HandleAsync(
      new(
        accountIdentifier,
        authorityId,
        request.ValueLimit,
        request.IsUnlimitedValueLimit,
        request.MinApprovers,
        request.ProfileIds ?? [],
        request.ActorId),
      cancellationToken);

    return result.ToActionResult(StatusCodes.Status201Created);
  }

  [EndpointSummary("Atualiza uma faixa de alçada")]
  [EndpointDescription("Atualiza limite, quantidade mínima de aprovadores e perfis elegíveis.")]
  [HttpPut("{roleId:guid}")]
  public async Task<IActionResult> UpdateAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    [FromBody] ApprovalRoleRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ApprovalRolesController][UpdateAsync] Received a request to update an approval role.");

    var result = await updateUseCase.HandleAsync(
      new(
        accountIdentifier,
        authorityId,
        roleId,
        request.ValueLimit,
        request.IsUnlimitedValueLimit,
        request.MinApprovers,
        request.ProfileIds ?? [],
        request.ActorId),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Ativa ou desativa uma faixa de alçada")]
  [EndpointDescription("Desativar a última faixa de um perfil requer confirmação explícita de elevação de privilégio.")]
  [HttpPatch("{roleId:guid}/activation")]
  public async Task<IActionResult> SetActivationAsync(
    string accountIdentifier,
    Guid authorityId,
    Guid roleId,
    [FromBody] ApprovalRoleActivationRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ApprovalRolesController][SetActivationAsync] Received a request to change approval role activation.");
    var result = await activationUseCase.HandleAsync(
      new(
        accountIdentifier,
        authorityId,
        roleId,
        request.Active,
        request.ConfirmPrivilegeElevation,
        request.UpdatedBy),
      cancellationToken);

    return result.ToActionResult();
  }
}

public sealed record ApprovalRoleRequest(
  decimal? ValueLimit,
  bool IsUnlimitedValueLimit,
  short MinApprovers,
  Guid[]? ProfileIds,
  string? ActorId);

public sealed record ApprovalRoleActivationRequest(
  bool Active,
  bool ConfirmPrivilegeElevation,
  string? UpdatedBy);
