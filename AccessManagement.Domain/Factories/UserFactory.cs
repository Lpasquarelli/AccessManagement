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

  public User Update(
    User user,
    string name,
    string? email,
    string? phone,
    string taxId,
    string authenticationId,
    bool isBrazilResident,
    string? updatedBy)
  {
    user.Name = name.Trim();
    user.Email = NormalizeOptional(email);
    user.Phone = NormalizeOptional(phone);
    user.TaxId = taxId.Trim();
    user.AuthenticationId = authenticationId.Trim();
    user.IsBrazilResident = isBrazilResident;
    user.UpdatedAt = DateTime.UtcNow;
    user.UpdatedBy = NormalizeOptional(updatedBy);

    return user;
  }

  public User Deactivate(User user, string? updatedBy)
  {
    user.Active = false;
    user.UpdatedAt = DateTime.UtcNow;
    user.UpdatedBy = updatedBy == null ? NormalizeOptional(updatedBy) : "UNKNOWN";

    return user;
  }

  private static string? NormalizeOptional(string? value) =>
    string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
