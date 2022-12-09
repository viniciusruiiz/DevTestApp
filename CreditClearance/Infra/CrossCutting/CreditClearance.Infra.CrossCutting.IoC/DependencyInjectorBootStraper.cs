using CreditClearance.Domain.Interfaces.Repository;
using CreditClearance.Domain.Interfaces.Service;
using CreditClearance.Infra.Data.Repository;
using CreditClearance.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CreditClearance.Infra.CrossCutting.IoC;

public static class DependencyInjectorBootStraper
{
    #region Methods
    public static void RegisterServices(this IServiceCollection services)
    {
        ConfigureRepositories(services);
        ConfigureServices(services);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICreditService, CreditService>();
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddSingleton<ICreditRepository, CreditRepository>();
    } 
    #endregion
}