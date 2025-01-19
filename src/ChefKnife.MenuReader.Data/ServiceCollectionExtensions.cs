using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using ChefKnife.MenuReader.Data.Repositories;

namespace ChefKnife.MenuReader.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDataBase(this IServiceCollection services, string connectionString)
    {
        var npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();

        //https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
        services.AddDbContext<MenuReaderDbContext>(builder =>
        {
            builder.UseNpgsql(npgsqlDataSource);
        });

        return services;
    }

    public static void RegisterDataServices(this IServiceCollection services)
    {
        // add generic repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}