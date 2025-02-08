using ChefKnife.Display.WebAPI;
using ChefKnife.HttpService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwagger();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpService, HttpService>();

var app = builder.Build();

app.ConfigureSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(policy =>
    policy.AllowAnyOrigin() 
        .AllowAnyMethod()
        .AllowAnyHeader()
);
app.UseCorsMiddleware();

app.Run();
