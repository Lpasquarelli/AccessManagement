using AccessManagement.Application.Results;

namespace AccessManagement.Application.UseCases;

public interface IUseCase<in TInput, TOutput>
  where TInput : IUseCaseInput
{
  Task<Result<TOutput>> HandleAsync(
    TInput input,
    CancellationToken cancellationToken = default);
}
