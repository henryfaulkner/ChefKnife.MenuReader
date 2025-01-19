using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChefKnife.MenuReader.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace ChefKnife.MenuReader.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDataBase(this IServiceCollection services, IConfiguration config)
    {
        var cosmosDbSection = config.GetSection("CosmosDB");

        //https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
        services.AddDbContext<MenuReaderDbContext>(builder =>
        {
            builder.UseCosmos(cosmosDbSection["AccountEndpoint"], cosmosDbSection["AccountKey"], cosmosDbSection["DataBaseName"]);
        });

        return services;
    }

    public static void RegisterDataServices(this IServiceCollection services)
    {
        // add generic repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}