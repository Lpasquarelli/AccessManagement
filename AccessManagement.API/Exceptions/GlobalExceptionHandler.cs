using AccessManagement.API.Contracts;
using AccessManagement.Application.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace AccessManagement.API.Exceptions;

public sealed class GlobalExceptionHandler(
  ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
  {
    var isUnavailable = exception is DataStoreUnavailableException;
    var statusCode = isUnavailable
      ? StatusCodes.Status503ServiceUnavailable
      : StatusCodes.Status500InternalServerError;
    var message = isUnavailable
      ? "The data service is temporarily unavailable."
      : "An unexpected error occurred.";

    logger.LogError(
      exception,
      "[GlobalExceptionHandler][TryHandleAsync] Unhandled request exception. TraceIdentifier: {TraceIdentifier}, Path: {Path}.",
      httpContext.TraceIdentifier,
      httpContext.Request.Path.Value);

    httpContext.Response.StatusCode = statusCode;
    await httpContext.Response.WriteAsJsonAsync(
      new ErrorResponse(message),
      cancellationToken);

    return true;
  }
}
