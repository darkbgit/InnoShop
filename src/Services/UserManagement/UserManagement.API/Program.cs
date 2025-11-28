using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Core.Extensions;
using UserManagement.API.Extensions;
using UserManagement.Core.DI;
using UserManagement.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

ServiceCollectionForInfrastructure.RegisterDependencies(builder.Services, builder.Configuration);
ServiceCollectionForCore.RegisterDependencies(builder.Services);

var reactApp = builder.Configuration["FrontendOptions:Url"] ??
    throw new ArgumentNullException("ReactApp is not configured");

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(reactApp);
    });
});

var jwtKey = builder.Configuration["Jwt:Key"] ??
    throw new ArgumentNullException("JWT key is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await app.SeedDatabaseAsync();
}

app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
