using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Intrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICartItemRepository, RedisCartItemRepository>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

var app = builder.Build();

app.MapGet("/", () => "Hello ShoppingCart Api!");

app.MapGet("/api/health", () => Results.Ok("Healthy"));

app.MapPost("/api/cart/items", async (CartItem item, ICartItemRepository repository) =>
{
    await repository.AddAsync(item);

    // Simulate adding an item to the cart
    return Results.Created($"/api/cart/{item}", new { Item = item });
});

app.Run();
