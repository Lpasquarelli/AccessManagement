namespace AccessManagement.Application.UseCases.Users.UpdateUser;

public sealed record UpdateUserUseCaseOutput(
  Guid Id,
  string Name,
  string? Email,
  string? Phone,
  string TaxId,
  string AuthenticationId,
  bool IsBrazilResident,
  bool Active,
  DateTime CreatedAt,
  DateTime? UpdatedAt,
  string? CreatedBy,
  string? UpdatedBy);
