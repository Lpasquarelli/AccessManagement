using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Users.AccountUsers.AddUserToAccount;
using AccessManagement.Application.UseCases.Users.AccountUsers.ListAccountUsers;
using AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountActivation;
using AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountProfiles;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/accounts/{accountIdentifier}/users")]
public sealed class AccountUsersController(
  IListAccountUsersUseCase listUseCase,
  IAddUserToAccountUseCase addUseCase,
  ISetUserAccountActivationUseCase activationUseCase,
  ISetUserAccountProfilesUseCase profilesUseCase,
  ILogger<AccountUsersController> logger) : ControllerBase
{
  [EndpointSummary("Lista os usuários da conta")]
  [EndpointDescription("Retorna uma lista direta, sem metadados de paginação. A conta é identificada por AccountIdentifier.")]
  [HttpGet]
  public async Task<IActionResult> ListAsync(
    string accountIdentifier,
    CancellationToken cancellationToken,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
  {
    logger.LogInformation(
      "[AccountUsersController][ListAsync] Received a request to list account users.");

    var result = await listUseCase.HandleAsync(
      new(accountIdentifier, page, pageSize),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Cria um usuário operador")]
  [EndpointDescription("Cria o usuário e o vínculo com a conta na mesma operação. O usuário não é titular nem mestre; os perfis são opcionais.")]
  [HttpPost]
  public async Task<IActionResult> AddAsync(
    string accountIdentifier,
    [FromBody] AddUserToAccountRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccountUsersController][AddAsync] Received a request to add a user to an account.");

    var result = await addUseCase.HandleAsync(
      new(
        accountIdentifier,
        request.Name,
        request.Email,
        request.Phone,
        request.TaxId,
        request.AuthenticationId,
        request.IsBrazilResident,
        request.ProfileIds ?? [],
        request.CreatedBy),
      cancellationToken);

    return result.ToActionResult(StatusCodes.Status201Created);
  }

  [EndpointSummary("Ativa ou desativa o vínculo do usuário")]
  [EndpointDescription("Desativa logicamente o acesso do usuário à conta sem remover o cadastro.")]
  [HttpPatch("{userAccountId:guid}/activation")]
  public async Task<IActionResult> SetActivationAsync(
    string accountIdentifier,
    Guid userAccountId,
    [FromBody] AccountUserActivationRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccountUsersController][SetActivationAsync] Received a request to change account user activation.");
    var result = await activationUseCase.HandleAsync(
      new(accountIdentifier, userAccountId, request.Active),
      cancellationToken);

    return result.ToActionResult();
  }

  [EndpointSummary("Substitui os perfis do usuário")]
  [EndpointDescription("A lista enviada representa o conjunto completo de perfis que permanecerá atribuído ao usuário na conta.")]
  [HttpPut("{userAccountId:guid}/profiles")]
  public async Task<IActionResult> SetProfilesAsync(
    string accountIdentifier,
    Guid userAccountId,
    [FromBody] SetUserAccountProfilesRequest request,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[AccountUsersController][SetProfilesAsync] Received a request to replace account user profiles.");

    var result = await profilesUseCase.HandleAsync(
      new(accountIdentifier, userAccountId, request.ProfileIds ?? [], request.UpdatedBy),
      cancellationToken);

    return result.ToActionResult();
  }
}

public sealed record AddUserToAccountRequest(
  string Name,
  string? Email,
  string? Phone,
  string TaxId,
  string AuthenticationId,
  bool IsBrazilResident,
  Guid[]? ProfileIds,
  string? CreatedBy);

public sealed record SetUserAccountProfilesRequest(
  Guid[]? ProfileIds,
  string? UpdatedBy);

public sealed record AccountUserActivationRequest(bool Active);
