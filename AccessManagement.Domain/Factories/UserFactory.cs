using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public sealed class UserFactory : IUserFactory
{
  public User Create(
    string name,
    string? email,
    string? phone,
    string taxId,
    string authenticationId,
    bool isBrazilResident,
    string? createdBy)
  {
    return new User
    {
      Name = name.Trim(),
      Email = NormalizeOptional(email),
      Phone = NormalizeOptional(phone),
      TaxId = taxId.Trim(),
      AuthenticationId = authenticationId.Trim(),
      IsBrazilResident = isBrazilResident,
      Active = true,
      CreatedBy = NormalizeOptional(createdBy)
    };
  }

  private static string? NormalizeOptional(string? value) =>
    string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
