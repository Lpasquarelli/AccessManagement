namespace AccessManagement.Application.Core.Exceptions;

public sealed class InvalidConfigurationException(string message)
  : Exception(message);
