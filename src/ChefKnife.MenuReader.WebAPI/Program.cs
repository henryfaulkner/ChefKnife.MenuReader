using ChefKnife.MenuReader.WebAPI;
using ChefKnife.MenuReader.Data;
using ChefKnife.Shared.Config;
using ChefKnife.MenuReader.StorageService;
using ChefKnife.MenuReader.DocumentProcessorService;
using Azure.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

if (!string.IsNullOrEmpty(builder.Configuration["KeyVaultUri"]))
{
    // Use DefaultAzureCredential for Managed Identity or Service Principal authentication
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUri"] ?? string.Empty), new DefaultAzureCredential());
    var keyVaultSecrets = builder.Configuration.AsEnumerable()
                                       .Where(kvp => kvp.Key.StartsWith("KV-"))
                                       .Where(kvp => !string.IsNullOrEmpty(kvp.Value)) // Filter out empty values
                                       .ToList();

    foreach (var kvp in keyVaultSecrets)
    {
        // Replace dashes with colons in the key name
        string adjustedKey = kvp.Key;
        // Remove KV-, then replace - with :
        adjustedKey.Remove(0, 3).Replace('-', ':');

        // Manually set the adjusted key-value pair back in the configuration
        builder.Configuration[adjustedKey] = kvp.Value;
    }
}

var cosmosDbSection = builder.Configuration.GetSection("CosmosDB");
builder.Services.ConfigureDataBase(
    new CosmosConfig(cosmosDbSection["AccountEndpoint"], cosmosDbSection["AccountKey"], cosmosDbSection["DataBaseName"])
);
builder.Services.RegisterDataServices();

var menuDumpStorageSection = builder.Configuration.GetSection("Storage:MenuDump");
builder.Services.RegisterAzureBlobStorage(
    new AzureBlobConfig(menuDumpStorageSection["ConnectionString"], menuDumpStorageSection["ContainerName"])
);

var documentIntelligenceSection = builder.Configuration.GetSection("DocumentProcessor:DocumentIntelligence");
builder.Services.RegisterAzureDocumentIntelligence(
    new AzureDocumentIntelligenceConfig(documentIntelligenceSection["Endpoint"], documentIntelligenceSection["ApiKey"], documentIntelligenceSection["ModelId"])
);

builder.Services.AddSwagger();

var app = builder.Build();

app.ConfigureSwagger();

app.UseCors(policy =>
    policy.AllowAnyOrigin() // Allowing all origins
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();