using ECommmerce.SharedLibrary.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using AuthenticationApi.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
namespace AuthenticationApi.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        //conextion a la base de datos
        //JWT add Authentication scheme

        SharedServiceContainer.AddSharedServices<AuthenticationDbContext>
            (services, config, config["MySerilog:FileName"]!);


        //Creamoas la dependencias
        services.AddScoped<IUser,UserRepository>();
        return services;

    }


public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
    {
        //Registramos los middlewares

        //Global Exception: para manejar erroes
        //Liste Only to Api gateWaye: Bloquea outsider

        SharedServiceContainer.UseSharedPolicies(app);
        return app;
    }
}