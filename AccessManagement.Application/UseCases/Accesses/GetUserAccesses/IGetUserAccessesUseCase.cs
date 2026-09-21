using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.UseCases.Accesses.GetUserAccesses;

public interface IGetUserAccessesUseCase
  : IUseCase<GetUserAccessesUseCaseInput, UserAccessSnapshot>;
