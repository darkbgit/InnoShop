using ProductManagement.API.Clients;
using ProductManagement.API.Extensions;
using ProductManagement.API.Handlers;
using ProductManagement.Core.DI;
using ProductManagement.DataAccess.DI;
using Refit;
using Shared.Core.Extensions;

namespace ProductManagement.API;

public class Program
{
public static async Task Main(string[] args)
{

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ServiceCollectionForDataAccess.RegisterDependencies(builder.Services, builder.Configuration);
ServiceCollectionForCore.RegisterDependencies(builder.Services);

var reactApp = builder.Configuration["ReactApp"] ?? 
    throw new ArgumentNullException("ReactApp is not configured");

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(reactApp);
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomHttpMessageHandler>();

var authClient = builder.Configuration["AuthClient"] ??
    throw new ArgumentNullException("AuthClient is not configured");

builder.Services.AddRefitClient<IAuthClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(authClient))
    .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                    return true;
                
                if (sender?.RequestUri?.Host == "users.api" && cert != null && cert.Subject.Contains("CN=localhost"))
                        return true;
        
                return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
        });;

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.ApplyMigration();
await app.SeedDatabaseAsync();

app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthFromRequestHeaderMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

}
}
