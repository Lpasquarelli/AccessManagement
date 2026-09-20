namespace AccessManagement.Application.UseCases.Users.GetUserById;

public sealed record GetUserByIdUseCaseOutput(
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
  Guid? UpdatedBy);
