namespace AccessManagement.Application.Core.Exceptions;

public sealed class DataStoreUnavailableException : Exception
{
  public DataStoreUnavailableException(string message, Exception? innerException = null)
    : base(message, innerException)
  {
  }
}
