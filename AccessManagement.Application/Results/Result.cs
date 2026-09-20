namespace AccessManagement.Application.Results;

public class Result
{
  protected Result(
    bool isSuccess,
    string? message,
    string[]? errors,
    ResultErrorType errorType)
  {
    IsSuccess = isSuccess;
    Message = message;
    Errors = errors is { Length: > 0 } ? errors : null;
    ErrorType = errorType;
  }

  public bool IsSuccess { get; }
  public string? Message { get; }
  public string[]? Errors { get; }
  public ResultErrorType ErrorType { get; }

  public static Result Ok() => new(true, null, null, ResultErrorType.None);

  public static Result Fail() =>
    new(false, "An unexpected error occurred.", null, ResultErrorType.Unexpected);

  public static Result Fail(
    string message,
    ResultErrorType errorType = ResultErrorType.Unexpected,
    string[]? errors = null) =>
    new(false, message, errors, errorType);
}

public sealed class Result<T> : Result
{
  private Result(
    bool isSuccess,
    T? data,
    string? message,
    string[]? errors,
    ResultErrorType errorType)
    : base(isSuccess, message, errors, errorType)
  {
    Data = data;
  }

  public T? Data { get; }

  public new static Result<T> Ok() =>
    new(true, default, null, null, ResultErrorType.None);

  public static Result<T> Ok(T data) =>
    new(true, data, null, null, ResultErrorType.None);

  public new static Result<T> Fail() =>
    new(false, default, "An unexpected error occurred.", null, ResultErrorType.Unexpected);

  public new static Result<T> Fail(
    string message,
    ResultErrorType errorType = ResultErrorType.Unexpected,
    string[]? errors = null) =>
    new(false, default, message, errors, errorType);
}
