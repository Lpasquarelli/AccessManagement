namespace AccessManagement.Domain.Factories;

internal static class FactoryValueNormalizer
{
  internal static string? Optional(string? value) =>
    string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
