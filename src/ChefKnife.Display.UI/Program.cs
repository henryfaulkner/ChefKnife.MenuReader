var corsPolicy = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseFileServer();

app.UseCors(corsPolicy);

app.Run();
