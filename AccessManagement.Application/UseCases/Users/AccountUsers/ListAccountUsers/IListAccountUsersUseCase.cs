namespace AccessManagement.Application.UseCases.Users.AccountUsers.ListAccountUsers;

public interface IListAccountUsersUseCase
  : IUseCase<ListAccountUsersUseCaseInput, IReadOnlyCollection<AccountUserOutput>>;
