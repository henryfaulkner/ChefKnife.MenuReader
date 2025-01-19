using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChefKnife.MenuReader.Data.Repositories;
using ChefKnife.MenuReader.Shared.Config;

namespace ChefKnife.MenuReader.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDataBase(this IServiceCollection services, CosmosConfig config)
    {
        //https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
        services.AddDbContext<MenuReaderDbContext>(builder =>
        {
            builder.UseCosmos(config.AccountEndpoint, config.AccountKey, config.DataBase);
        });

        return services;
    }

    public static void RegisterDataServices(this IServiceCollection services)
    {
        // add generic repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}