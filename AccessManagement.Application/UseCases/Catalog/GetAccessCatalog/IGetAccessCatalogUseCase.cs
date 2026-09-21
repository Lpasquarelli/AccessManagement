using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.UseCases.Catalog.GetAccessCatalog;

public interface IGetAccessCatalogUseCase : IUseCase<GetAccessCatalogUseCaseInput, AccessCatalog>;
