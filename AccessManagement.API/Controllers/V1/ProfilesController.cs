using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Profiles.CreateProfile;
using AccessManagement.Application.UseCases.Profiles.ListProfiles;
using AccessManagement.Application.UseCases.Profiles.SetProfileActivation;
using AccessManagement.Application.UseCases.Profiles.SetProfilePermissions;
using AccessManagement.Application.UseCases.Profiles.UpdateProfile;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/accounts/{accountIdentifier}/profiles")]
public sealed class ProfilesController(
  IListProfilesUseCase listUseCase,
  ICreateProfileUseCase createUseCase,
  IUpdateProfileUseCase updateUseCase,
  ISetProfileActivationUseCase activationUseCase,
  ISetProfilePermissionsUseCase permissionsUseCase,
  ILogger<ProfilesController> logger) : ControllerBase
{
  [EndpointSummary("Lista os perfis da conta")]
  [EndpointDescription("Retorna diretamente os perfis ativos pertencentes ao AccountIdentifier informado na rota.")]
  [HttpGet]
  public async Task<IActionResult> ListAsync(
    string accountIdentifier,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ProfilesController][ListAsync] Received a request to list profiles.");

    var result = await listUseCase.HandleAsync(
      new(accountIdentifier),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Cria um perfil")]
  [EndpointDescription("Cria um perfil isolado na conta e pode atribuir permissões iniciais.")]
  [HttpPost]
  public async Task<IActionResult> CreateAsync(
    string accountIdentifier,
    [FromBody] CreateProfileRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ProfilesController][CreateAsync] Received a request to create a profile.");

    var result = await createUseCase.HandleAsync(
      new(accountIdentifier, request.Name, request.Description, request.PermissionIds ?? [], request.CreatedBy),
      cancellationToken);

    return result.ToActionResult(StatusCodes.Status201Created);
  }

  [EndpointSummary("Atualiza um perfil")]
  [EndpointDescription("Atualiza somente os dados cadastrais do perfil dentro da conta informada.")]
  [HttpPut("{profileId:guid}")]
  public async Task<IActionResult> UpdateAsync(
    string accountIdentifier,
    Guid profileId,
    [FromBody] UpdateProfileRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ProfilesController][UpdateAsync] Received a request to update a profile.");

    var result = await updateUseCase.HandleAsync(
      new(accountIdentifier, profileId, request.Name, request.Description, request.UpdatedBy),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Ativa ou desativa um perfil")]
  [EndpointDescription("Altera o estado lógico do perfil, preservando suas configurações.")]
  [HttpPatch("{profileId:guid}/activation")]
  public async Task<IActionResult> SetActivationAsync(
    string accountIdentifier,
    Guid profileId,
    [FromBody] ActivationRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ProfilesController][SetActivationAsync] Received a request to change profile activation.");

    var result = await activationUseCase.HandleAsync(
      new(accountIdentifier, profileId, request.Active, request.UpdatedBy),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Substitui as permissões do perfil")]
  [EndpointDescription("A lista enviada representa o conjunto completo de permissões que permanecerá no perfil.")]
  [HttpPut("{profileId:guid}/permissions")]
  public async Task<IActionResult> SetPermissionsAsync(
    string accountIdentifier,
    Guid profileId,
    [FromBody] SetPermissionsRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[ProfilesController][SetPermissionsAsync] Received a request to replace profile permissions.");
    var result = await permissionsUseCase.HandleAsync(
      new(accountIdentifier, profileId, request.PermissionIds ?? []),
      cancellationToken);

    return result.ToActionResult();
  }
}

public sealed record CreateProfileRequest(
  string Name,
  string? Description,
  Guid[]? PermissionIds,
  string? CreatedBy);

public sealed record UpdateProfileRequest(string Name, string? Description, string? UpdatedBy);

public sealed record ActivationRequest(bool Active, string? UpdatedBy);

public sealed record SetPermissionsRequest(Guid[]? PermissionIds);
