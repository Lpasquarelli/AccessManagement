using AccessManagement.API.Contracts;
using AccessManagement.API.Extensions;
using AccessManagement.Application.UseCases.Users.GetUserById;
using AccessManagement.Application.UseCases.Users.InsertUser;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/users")]
public sealed class UserController(
  IInsertUserUseCase insertUserUseCase,
  IGetUserByIdUseCase getUserByIdUseCase,
  ILogger<UserController> logger) : ControllerBase
{
  [HttpPost]
  [ProducesResponseType(typeof(InsertUserUseCaseOutput), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> InsertAsync(
    [FromBody] InsertUserUseCaseInput input,
    CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "[UserController][InsertAsync] Received a request to create a user.");

    var result = await insertUserUseCase.HandleAsync(input, cancellationToken);
    return result.ToActionResult(StatusCodes.Status201Created);
  }

  [HttpGet("{id:guid}")]
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
}
