using AccessManagement.Application.UseCases.Accesses.EvaluateAccountAccess;
using AccessManagement.Application.UseCases.Accesses.GetUserAccesses;
using AccessManagement.Application.UseCases.ApprovalRoles.CreateApprovalRole;
using AccessManagement.Application.UseCases.ApprovalRoles.ListApprovalRoles;
using AccessManagement.Application.UseCases.ApprovalRoles.SetApprovalRoleActivation;
using AccessManagement.Application.UseCases.ApprovalRoles.UpdateApprovalRole;
using AccessManagement.Application.UseCases.Catalog.GetAccessCatalog;
using AccessManagement.Application.UseCases.Profiles.CreateProfile;
using AccessManagement.Application.UseCases.Profiles.ListProfiles;
using AccessManagement.Application.UseCases.Profiles.SetProfileActivation;
using AccessManagement.Application.UseCases.Profiles.SetProfilePermissions;
using AccessManagement.Application.UseCases.Profiles.UpdateProfile;
using AccessManagement.Application.UseCases.Users.AccountUsers.AddUserToAccount;
using AccessManagement.Application.UseCases.Users.AccountUsers.ListAccountUsers;
using AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountActivation;
using AccessManagement.Application.UseCases.Users.AccountUsers.SetUserAccountProfiles;
using AccessManagement.Application.UseCases.Users.DeleteUser;
using AccessManagement.Application.UseCases.Users.GetUserById;
using AccessManagement.Application.UseCases.Users.UpdateUser;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AccessManagement.Application.Di;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddTransient<IUserFactory, UserFactory>();
    services.AddTransient<AccessContextFactory>();
    services.AddTransient<AccountFactory>();
    services.AddTransient<PermissionFactory>();
    services.AddTransient<AuthorityFactory>();
    services.AddTransient<UserAccountFactory>();
    services.AddTransient<ProfileFactory>();
    services.AddTransient<PermissionGroupFactory>();
    services.AddTransient<UserAccountProfileFactory>();
    services.AddTransient<ProfilePermissionFactory>();
    services.AddTransient<AuthorityApprovalRoleFactory>();
    services.AddTransient<ApprovalRoleProfileFactory>();
    services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
    services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
    services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
    services.AddScoped<IGetUserAccessesUseCase, GetUserAccessesUseCase>();
    services.AddScoped<IEvaluateAccountAccessUseCase, EvaluateAccountAccessUseCase>();
    services.AddScoped<IGetAccessCatalogUseCase, GetAccessCatalogUseCase>();
    services.AddScoped<IListProfilesUseCase, ListProfilesUseCase>();
    services.AddScoped<ICreateProfileUseCase, CreateProfileUseCase>();
    services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
    services.AddScoped<ISetProfileActivationUseCase, SetProfileActivationUseCase>();
    services.AddScoped<ISetProfilePermissionsUseCase, SetProfilePermissionsUseCase>();
    services.AddScoped<IListAccountUsersUseCase, ListAccountUsersUseCase>();
    services.AddScoped<IAddUserToAccountUseCase, AddUserToAccountUseCase>();
    services.AddScoped<ISetUserAccountActivationUseCase, SetUserAccountActivationUseCase>();
    services.AddScoped<ISetUserAccountProfilesUseCase, SetUserAccountProfilesUseCase>();
    services.AddScoped<IListApprovalRolesUseCase, ListApprovalRolesUseCase>();
    services.AddScoped<ICreateApprovalRoleUseCase, CreateApprovalRoleUseCase>();
    services.AddScoped<IUpdateApprovalRoleUseCase, UpdateApprovalRoleUseCase>();
    services.AddScoped<ISetApprovalRoleActivationUseCase, SetApprovalRoleActivationUseCase>();
    return services;
  }
}
