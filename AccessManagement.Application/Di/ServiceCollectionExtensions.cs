using AccessManagement.Application.UseCases.Users.GetUserById;
using AccessManagement.Application.UseCases.Users.InsertUser;
using AccessManagement.Domain.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AccessManagement.Application.Di;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddTransient<IUserFactory, UserFactory>();
    services.AddScoped<IInsertUserUseCase, InsertUserUseCase>();
    services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();

    return services;
  }
}
