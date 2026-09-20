using System.Net.Mail;
using System.Text.Json.Serialization;

namespace AccessManagement.Application.UseCases.Users.UpdateUser;

public sealed record UpdateUserUseCaseInput(
  Guid Id,
  string Name,
  string? Email,
  string? Phone,
  string TaxId,
  string AuthenticationId,
  bool IsBrazilResident,
  string? UpdatedBy) : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput()
  {
    var errors = new List<string>();

    if (Id == Guid.Empty)
      errors.Add("Id must be a non-empty GUID.");

    if (string.IsNullOrWhiteSpace(Name))
      errors.Add("Name is required.");
    else if (Name.Trim().Length > 150)
      errors.Add("Name must contain at most 150 characters.");

    if (!string.IsNullOrWhiteSpace(Email))
    {
      if (Email.Trim().Length > 320)
        errors.Add("Email must contain at most 320 characters.");
      else if (!MailAddress.TryCreate(Email.Trim(), out _))
        errors.Add("Email must be valid.");
    }

    if (!string.IsNullOrWhiteSpace(Phone) && Phone.Trim().Length > 32)
      errors.Add("Phone must contain at most 32 characters.");

    if (string.IsNullOrWhiteSpace(TaxId))
      errors.Add("TaxId is required.");
    else if (TaxId.Trim().Length > 20)
      errors.Add("TaxId must contain at most 20 characters.");

    if (string.IsNullOrWhiteSpace(AuthenticationId))
      errors.Add("AuthenticationId is required.");
    else if (AuthenticationId.Trim().Length > 150)
      errors.Add("AuthenticationId must contain at most 150 characters.");

    if (!string.IsNullOrWhiteSpace(UpdatedBy) && UpdatedBy.Trim().Length > 150)
      errors.Add("UpdatedBy must contain at most 150 characters.");

    return (errors.Count == 0, errors.ToArray());
  }
}
