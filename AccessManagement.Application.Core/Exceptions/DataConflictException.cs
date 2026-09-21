namespace AccessManagement.Application.Core.Exceptions;

public sealed class DataConflictException(string message, Exception? innerException = null)
  : Exception(message, innerException);
