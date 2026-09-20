namespace AccessManagement.Application.UseCases;

public interface IUseCaseInput
{
  (bool IsValid, string[] Errors) ValidateInput();
}
