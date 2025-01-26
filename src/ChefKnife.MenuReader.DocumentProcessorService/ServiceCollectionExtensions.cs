using ChefKnife.Shared.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.DocumentProcessorService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterAzureDocumentIntelligence(this IServiceCollection services, AzureDocumentIntelligenceConfig config)
    {
        services.AddTransient<IDocumentProcessorService>(
            builder => new AzureDocumentIntelligenceService(config)
        );

        return services;
    }
}
