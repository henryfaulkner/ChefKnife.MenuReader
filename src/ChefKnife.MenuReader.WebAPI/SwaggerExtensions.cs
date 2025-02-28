﻿using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ChefKnife.MenuReader.WebAPI;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuReader API", Version = "v1" });
            c.EnableAnnotations();
        });
    }

    public static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        // Save local swagger json for application gateway
        if (app.Environment.IsDevelopment())
        {
            app.Services.GenerateSwaggerFile();
        }
    }

    public static void GenerateSwaggerFile(this IServiceProvider serviceProvider, string fileName = "swagger.json")
    {
        var swaggerProvider = serviceProvider.GetRequiredService<ISwaggerProvider>();
        var doc = swaggerProvider.GetSwagger("v1", null, "/");
        var swaggerFile = doc.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
        File.WriteAllText(fileName, swaggerFile);
    }
}
