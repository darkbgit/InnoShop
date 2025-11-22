using ProductManagement.API.Clients;
using ProductManagement.API.Extensions;
using ProductManagement.API.Handlers;
using ProductManagement.Core.DI;
using ProductManagement.DataAccess.DI;
using Refit;
using Shared.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ServiceCollectionForDataAccess.RegisterDependencies(builder.Services, builder.Configuration);
ServiceCollectionForCore.RegisterDependencies(builder.Services);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomHttpMessageHandler>();

var authClient = builder.Configuration["AuthClient"] ??
    throw new ArgumentNullException("AuthClient is not configured");

builder.Services.AddRefitClient<IAuthClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(authClient));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await app.SeedDatabaseAsync();
}

app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthFromRequestHeaderMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
