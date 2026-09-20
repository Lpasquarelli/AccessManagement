using AccessManagement.Domain.Entities;

namespace AccessManagement.Domain.Factories;

public interface IUserFactory
{
  User Create(
    string name,
    string? email,
    string? phone,
    string taxId,
    string authenticationId,
    bool isBrazilResident,
    string? createdBy);

  User Update(
    User user,
    string name,
    string? email,
    string? phone,
    string taxId,
    string authenticationId,
    bool isBrazilResident,
    string? updatedBy);

  User Deactivate(User user, string? updatedBy);
}
