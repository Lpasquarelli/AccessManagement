namespace AccessManagement.Application.UseCases.Profiles.SetProfilePermissions;

public sealed record SetProfilePermissionsUseCaseOutput(Guid ProfileId, IReadOnlyCollection<Guid> PermissionIds);
