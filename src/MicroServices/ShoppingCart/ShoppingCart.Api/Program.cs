using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
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

// dotnet add package Microsoft.Extensions.ServiceDiscovery
// Dodaj odnajdywanie us³ug
builder.Services.AddServiceDiscovery();


builder.Services.AddHttpClient("OrderingApi", client =>
{
    // TODO: Zastosuj mechanizm atomatycznego odkrywania us³ug
    // https://learn.microsoft.com/en-us/dotnet/core/extensions/service-discovery?tabs=dotnet-cli
    client.BaseAddress = new Uri("https://ordering"); // 
}).AddServiceDiscovery();

// Dodaj obs³ugê sesji
builder.Services.AddDistributedMemoryCache(); // Przechowywanie sesji w pamiêci
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas wygaœniêcia sesji
    options.Cookie.HttpOnly = true; // Bezpieczeñstwo ciasteczka
    options.Cookie.IsEssential = true; // Wymagane ciasteczko
});

builder.Services.AddHealthChecks()
    .AddRedis(sp => sp.GetRequiredService<IConnectionMultiplexer>());

var app = builder.Build();

// U¿yj middleware sesji
app.UseSession();

app.MapGet("/", () => "Hello ShoppingCart Api!");

app.MapGet("/api/health", () => Results.Ok("Healthy"));

app.MapPost("/api/cart/items", async (CartItem item, ICartItemRepository repository, HttpContext context) =>
{
    // Pobierz sessionId z HttpContext.Session
    string sessionId = context.Session.Id;

    // Jeœli sessionId jest pusty, u¿yj sta³ego sessionId
    if (string.IsNullOrEmpty(sessionId))
    {
        sessionId = "default-session-001"; // Sta³y sessionId jako fallback
    }

    await repository.AddAsync(sessionId, item);

    // Simulate adding an item to the cart
    return Results.Created($"/api/cart/{item}", new { Item = item });
});

app.MapPost("/api/cart/checkout", async (IConnectionMultiplexer connectionMultiplexer, IHttpClientFactory factory, HttpContext context) =>
{
    var db = connectionMultiplexer.GetDatabase();

    // Pobierz sessionId
    var sessionId = context.Session.Id;

    // Jeœli sessionId jest pusty, u¿yj sta³ego sessionId
    if (string.IsNullOrEmpty(sessionId))
    {
        sessionId = "default-session-001"; // Sta³y sessionId jako fallback
    }

    string key = $"cart:{sessionId}";

    var cartItems = await db.HashGetAllAsync(key);

    OrderItem[] orderItems = cartItems.Select(item => new OrderItem
    {
        Id = int.Parse(item.Name.ToString().Split(':')[1]),
        Quantity = (int)item.Value
    }).ToArray();


    var httpClient = factory.CreateClient("OrderingApi");

    var response = await httpClient.PostAsJsonAsync("/api/orders", orderItems);

    if (!response.IsSuccessStatusCode)
    {
        return Results.BadRequest(new { Error = "Failed to create order" });
    }

    return Results.Ok(cartItems);
});



app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(report);
    }
});


app.Run();


public record OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}