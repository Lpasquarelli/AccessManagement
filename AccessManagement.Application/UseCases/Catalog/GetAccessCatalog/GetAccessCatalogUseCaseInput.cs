namespace AccessManagement.Application.UseCases.Catalog.GetAccessCatalog;

public sealed record GetAccessCatalogUseCaseInput : IUseCaseInput
{
  public (bool IsValid, string[] Errors) ValidateInput() => (true, []);
}
