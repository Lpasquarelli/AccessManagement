namespace AccessManagement.Application.UseCases.Profiles.SetProfileActivation;

public sealed record SetProfileActivationUseCaseOutput(Guid ProfileId, bool Active);
