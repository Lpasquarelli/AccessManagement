using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Accesses.EvaluateAccountAccess;
using AccessManagement.Application.UseCases.Accesses.GetUserAccesses;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/accesses")]
public sealed class AccessesController(
  IGetUserAccessesUseCase getAccessesUseCase,
  IEvaluateAccountAccessUseCase evaluateUseCase,
  ILogger<AccessesController> logger) : ControllerBase
{
  [EndpointSummary("Consulta os acessos efetivos do usuário")]
  [EndpointDescription("AuthenticationId é o documento ou e-mail usado pelo serviço externo de login. A resposta é direta, sem envelope.")]
  [HttpGet("users/{authenticationId}")]
  public async Task<IActionResult> GetUserAccessesAsync(
    string authenticationId,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccessesController][GetUserAccessesAsync] Received a request to query effective user access.");
    var result = await getAccessesUseCase.HandleAsync(
      new(authenticationId),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Avalia o acesso e a alçada do usuário")]
  [EndpointDescription("Quando amount é informado, seleciona a política aplicável e informa canApproveAlone para aquela operação.")]
  [HttpGet("users/{authenticationId}/accounts/{accountIdentifier}/evaluation")]
  public async Task<IActionResult> EvaluateAccountAccessAsync(
    string authenticationId,
    string accountIdentifier,
    [FromQuery] Guid permissionId,
    [FromQuery] Guid authorityId,
    [FromQuery] decimal? amount,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccessesController][EvaluateAccountAccessAsync] Received a request to evaluate account access rules.");
    var result = await evaluateUseCase.HandleAsync(
      new(
        authenticationId,
        accountIdentifier,
        permissionId,
        authorityId,
        amount),
      cancellationToken);

    return result.ToActionResult();
  }
}
