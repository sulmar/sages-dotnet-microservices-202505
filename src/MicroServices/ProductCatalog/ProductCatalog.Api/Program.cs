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

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins("https://localhost:7108").WithMethods("GET").AllowAnyHeader();

    // policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello Products.Api!");

app.MapGet("/ping", () => "Pong!");

app.MapGet("/api/products", async (IProductRepository repository) => await repository.GetAllAsync());

app.Run();
