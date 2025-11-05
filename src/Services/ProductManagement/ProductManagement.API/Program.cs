using ProductManagement.API.Extensions;
using ProductManagement.API.Middlewares;
using ProductManagement.Core.DI;
using ProductManagement.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ServiceCollectionForDataAccess.RegisterDependencies(builder.Services, builder.Configuration);
ServiceCollectionForCore.RegisterDependencies(builder.Services);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await app.SeedDatabaseAsync();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
