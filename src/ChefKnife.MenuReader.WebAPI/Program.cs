using ChefKnife.MenuReader.WebAPI;
using ChefKnife.MenuReader.Data;
using ChefKnife.MenuReader.Shared.Config;
using ChefKnife.MenuReader.StorageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var cosmosDbSection = builder.Configuration.GetSection("CosmosDB");
builder.Services.ConfigureDataBase(
    new CosmosConfig(cosmosDbSection["AccountEndpoint"], cosmosDbSection["AccountKey"], cosmosDbSection["DataBaseName"])
);
builder.Services.RegisterDataServices();

var menuDumpStorageSection = builder.Configuration.GetSection("Storage:MenuDump");
builder.Services.RegisterAzureBlobStorage(
    new AzureBlobConfig(menuDumpStorageSection["ConnectionString"], menuDumpStorageSection["ContainerName"])
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