using AccessManagement.API.Contracts;
using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Users.DeleteUser;
using AccessManagement.Application.UseCases.Users.GetUserById;
using AccessManagement.Application.UseCases.Users.UpdateUser;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public sealed class UserController(
  IGetUserByIdUseCase getUserByIdUseCase,
  IUpdateUserUseCase updateUserUseCase,
  IDeleteUserUseCase deleteUserUseCase,
  ILogger<UserController> logger) : ControllerBase
{
  [HttpGet("{id:guid}")]
  [EndpointSummary("Consulta um usuário por identificador")]
  [EndpointDescription("Retorna um usuário ativo pelo identificador técnico do cadastro.")]
  [ProducesResponseType(typeof(GetUserByIdUseCaseOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[UserController][GetByIdAsync] Received a request to query a user.");

    var result = await getUserByIdUseCase.HandleAsync(
      new GetUserByIdUseCaseInput(id),
      cancellationToken);

    return result.ToActionResult();
  }

  [HttpPut]
  [EndpointSummary("Atualiza um usuário")]
  [EndpointDescription("Atualiza os dados cadastrais de um usuário já existente.")]
  [ProducesResponseType(typeof(UpdateUserUseCaseOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdateAsync(
    [FromBody] UpdateUserUseCaseInput input,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[UserController][UpdateAsync] Received a request to update a user.");

    var result = await updateUserUseCase.HandleAsync(
      input,
      cancellationToken);

    return result.ToActionResult();
  }

  [HttpDelete("{id:guid}")]
  [EndpointSummary("Desativa um usuário")]
  [EndpointDescription("Executa a exclusão lógica do usuário, preservando o histórico do cadastro.")]
  [ProducesResponseType(typeof(DeleteUserUseCaseOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteAsync(
    Guid id,
    [FromQuery] string? updatedBy,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[UserController][DeleteAsync] Received a request to logically delete a user.");

    var result = await deleteUserUseCase.HandleAsync(
      new DeleteUserUseCaseInput(id, updatedBy),
      cancellationToken);

    return result.ToActionResult();
  }
}
