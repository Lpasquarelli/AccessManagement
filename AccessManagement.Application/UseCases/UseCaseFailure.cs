using AccessManagement.Application.Core.Exceptions;
using AccessManagement.Application.Results;

namespace AccessManagement.Application.UseCases;

internal static class UseCaseFailure
{
  public static Result<T> From<T>(Exception exception) => exception switch
  {
    DataConflictException => Result<T>.Fail(exception.Message, ResultErrorType.Conflict),
    InvalidConfigurationException => Result<T>.Fail(exception.Message, ResultErrorType.Unprocessable),
    DataStoreUnavailableException => Result<T>.Fail(
      "A required service is temporarily unavailable.",
      ResultErrorType.Unavailable),
    KeyNotFoundException => Result<T>.Fail("The requested resource was not found.", ResultErrorType.NotFound),
    _ => Result<T>.Fail()
  };
}
