namespace AccessManagement.Application.UseCases.Users.InsertUser;

public sealed record InsertUserUseCaseOutput(
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
