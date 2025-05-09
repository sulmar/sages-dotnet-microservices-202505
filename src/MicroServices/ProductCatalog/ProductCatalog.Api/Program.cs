using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.HealthChecks;
using ProductCatalog.Api.Mappers;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Context>(sp =>
{
    var products = new List<Product>
        {
            new Product(1, "Popular Product", 80.00m),
            new Product(2, "Special Item", 80.00m, 50m),
            new Product(3, "Extra Product", 80.00m),
            new Product(4, "Bonus Product", 80.00m, 70m),
            new Product(5, "Fancy Product", 80.00m, 70m),
            new Product(6, "Smart Product", 99.99m),
            new Product(7, "Old-school Product", 199.00m),
            new Product(8, "Future Product", 1.00m)
        };

    return new Context { Products = products.ToDictionary(p => p.Id) };
});

builder.Services.AddScoped<IProductRepository, DbProductRepository>();
builder.Services.AddScoped<IProductRepository, InMemoryProductRepository>();

builder.Services.AddSingleton<ProductMapper>();

// Health Check
builder.Services.AddHealthChecks()
    .AddCheck("Ping", () => HealthCheckResult.Healthy("Pong"))
    .AddCheck("Random", () =>
    {
        if (DateTime.Now.Minute % 2 == 0)
        {
            return HealthCheckResult.Healthy("Even minute");
        }
        else
        {
            return HealthCheckResult.Unhealthy("Odd minute");
        }
     }
    )
    .AddCheck<CustomHealthCheck>("Custom", HealthStatus.Degraded);

string secretKey = "a-string-secret-at-least-256-bits-long";

// dotnet add pacage Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://sages.pl",
            ValidAudience = "https://example.com",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello Products.Api!");


app.MapGet("/ping", () => "Pong!");

app.MapGet("/api/products", async (IProductRepository repository) => await repository.GetAllAsync())
    .RequireAuthorization();

app.MapPost("/api/products", async (IProductRepository repository, ProductDto productDto, ProductMapper mapper) =>
{
    Product product = mapper.MapToEntity(productDto);

    return Results.Created();
});

// app.MapHealthChecks("/health");


app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(report);
    }
});

app.Run();
