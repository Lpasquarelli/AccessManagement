using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Catalog.GetAccessCatalog;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/access-catalog")]
public sealed class AccessCatalogController(
  IGetAccessCatalogUseCase useCase,
  ILogger<AccessCatalogController> logger) : ControllerBase
{
  [EndpointSummary("Consulta o catálogo de acessos")]
  [EndpointDescription("Cada grupo informa hasApprovalPrivileges quando contém ao menos uma permissão de aprovação.")]
  [HttpGet]
  public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccessCatalogController][GetAsync] Received a request to query the access catalog.");

    var result = await useCase.HandleAsync(
      new GetAccessCatalogUseCaseInput(),
      cancellationToken);

    return result.ToActionResult();
  }
}
