using AccessManagement.Application.Core.Repositories;
using AccessManagement.Application.Results;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.Logging;

namespace AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;

public sealed class CreateApprovalRoleUseCase(
  IApprovalRoleRepository repository,
  AuthorityApprovalRoleFactory factory,
  ILogger<CreateApprovalRoleUseCase> logger) : ICreateApprovalRoleUseCase
{
  public async Task<Result<CreateApprovalRoleUseCaseOutput>> HandleAsync(
    CreateApprovalRoleUseCaseInput input,
    CancellationToken cancellationToken = default)
  {
    logger.LogInformation(
      "[CreateApprovalRoleUseCase][HandleAsync] Creating an approval role.");

    var validation = input.ValidateInput();
    if (!validation.IsValid)
    {
      return Result<CreateApprovalRoleUseCaseOutput>.Fail(
        "The input is invalid.",
        ResultErrorType.Validation,
        validation.Errors);
    }

    try
    {
      var profileIds = input.ProfileIds.Distinct().ToArray();
      var role = factory.Create(
        input.AuthorityId,
        input.ValueLimit,
        input.IsUnlimitedValueLimit,
        input.MinApprovers,
        input.CreatedBy);

      var created = await repository.InsertAsync(
        input.AccountIdentifier,
        role,
        profileIds,
        cancellationToken);

      if (created is null)
      {
        return Result<CreateApprovalRoleUseCaseOutput>.Fail(
          "The authority was not found.",
          ResultErrorType.NotFound);
      }

      return Result<CreateApprovalRoleUseCaseOutput>.Ok(
        new(
          created.Id,
          created.AuthorityId,
          created.ValueLimit,
          created.IsUnlimitedValueLimit,
          created.MinApprovers,
          created.Active,
          profileIds));
    }
    catch (Exception exception)
    {
      logger.LogError(
        exception,
        "[CreateApprovalRoleUseCase][HandleAsync] Approval role creation failed.");

      return UseCaseFailure.From<CreateApprovalRoleUseCaseOutput>(exception);
    }
  }
}
