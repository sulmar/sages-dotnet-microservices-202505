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

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return connectionMultiplexer.GetDatabase();
});

builder.Services.AddHttpClient("OrderingApi", client =>
{
    // TODO: Zastosuj mechanizm atomatycznego odkrywania us³ug
    // https://learn.microsoft.com/en-us/dotnet/core/extensions/service-discovery?tabs=dotnet-cli
    client.BaseAddress = new Uri("https://localhost:7263");
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

app.MapPost("/api/cart/checkout", async (IConnectionMultiplexer connectionMultiplexer, IHttpClientFactory factory) =>
{
    var db = connectionMultiplexer.GetDatabase();

    string sessionId = "001abc";

    string key = $"cart:{sessionId}";

    var cartItems = await db.HashGetAllAsync(key);

    OrderItem[] orderItems = cartItems.Select(item => new OrderItem
    {
        Id = int.Parse(item.Name.ToString().Split(':')[1]),
        Quantity = (int)item.Value
    }).ToArray();


    var httpClient = factory.CreateClient("OrderingApi");

    await httpClient.PostAsJsonAsync("/api/orders", orderItems);

    return Results.Ok(cartItems);
});

app.Run();


public record OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}