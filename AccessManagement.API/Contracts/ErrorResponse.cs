using System.Text.Json.Serialization;

namespace AccessManagement.API.Contracts;

public sealed record ErrorResponse(
  string Message,
  [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  string[]? Errors = null);
