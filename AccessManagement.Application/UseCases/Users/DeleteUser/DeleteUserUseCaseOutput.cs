namespace AccessManagement.Application.UseCases.Users.DeleteUser;

public sealed record DeleteUserUseCaseOutput(
  Guid Id,
  bool Active,
  DateTime? UpdatedAt,
  string? UpdatedBy);
