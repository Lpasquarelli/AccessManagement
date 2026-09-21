namespace AccessManagement.Application.UseCases.Profiles.ListProfiles;

public interface IListProfilesUseCase : IUseCase<ListProfilesUseCaseInput, IReadOnlyCollection<ProfileOutput>>;
