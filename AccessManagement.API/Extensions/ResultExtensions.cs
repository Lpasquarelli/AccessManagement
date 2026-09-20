using AccessManagement.API.Contracts;
using AccessManagement.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagement.API.Extensions;

public static class ResultExtensions
{
  public static IActionResult ToActionResult(
    this Result result,
    int successStatusCode = StatusCodes.Status200OK)
  {
    if (result.IsSuccess)
      return new StatusCodeResult(successStatusCode);

    return CreateFailureResult(result);
  }

  public static IActionResult ToActionResult<T>(
    this Result<T> result,
    int successStatusCode = StatusCodes.Status200OK)
  {
    if (!result.IsSuccess)
      return CreateFailureResult(result);

    if (result.Data is null)
      return new StatusCodeResult(successStatusCode);

    return new ObjectResult(result.Data)
    {
      StatusCode = successStatusCode
    };
  }

  private static IActionResult CreateFailureResult(Result result)
  {
    var statusCode = result.ErrorType switch
    {
      ResultErrorType.Validation => StatusCodes.Status400BadRequest,
      ResultErrorType.NotFound => StatusCodes.Status404NotFound,
      ResultErrorType.Unavailable => StatusCodes.Status503ServiceUnavailable,
      _ => StatusCodes.Status500InternalServerError
    };

    return new ObjectResult(new ErrorResponse(
      result.Message ?? "An unexpected error occurred.",
      result.Errors))
    {
      StatusCode = statusCode
    };
  }
}
