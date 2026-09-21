namespace AccessManagement.Application.Results;

public enum ResultErrorType
{
  None = 0,
  Validation = 1,
  NotFound = 2,
  Unavailable = 3,
  Unexpected = 4,
  Conflict = 5,
  Unprocessable = 6
}
