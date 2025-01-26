using Microsoft.Extensions.DependencyInjection;
using ChefKnife.Shared.Config;

namespace ChefKnife.MenuReader.StorageService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterAzureBlobStorage(this IServiceCollection services, AzureBlobConfig config)
    {
        services.AddTransient<IStorageService>(
            builder => new AzureBlobService(config)
        );

        return services;
    }
}
