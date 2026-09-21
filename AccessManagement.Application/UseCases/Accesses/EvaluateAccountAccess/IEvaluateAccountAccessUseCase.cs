using AccessManagement.Application.Core.Models;

namespace AccessManagement.Application.UseCases.Accesses.EvaluateAccountAccess;

public interface IEvaluateAccountAccessUseCase
  : IUseCase<EvaluateAccountAccessUseCaseInput, AccountAccessEvaluation>;
