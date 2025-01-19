using ChefKnife.MenuReader.WebAPI;
using ChefKnife.MenuReader.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var dbConnectionString = builder.Configuration.GetConnectionString("AuditDB");
if (!string.IsNullOrEmpty(dbConnectionString))
    builder.Services.ConfigureDataBase(dbConnectionString);
builder.Services.RegisterDataServices();

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