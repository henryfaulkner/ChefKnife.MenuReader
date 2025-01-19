using ChefKnife.MenuReader.WebAPI;
using ChefKnife.MenuReader.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureDataBase(builder.Configuration);
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