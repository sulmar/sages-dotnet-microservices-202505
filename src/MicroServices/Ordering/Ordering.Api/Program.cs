var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Ordering.Api!");

app.MapPost("/api/orders", (OrderItem[] reqest) =>
{
    // Simulate creating an order
    return Results.Created("/api/orders/1", new { OrderId = 1, Status = "Created" });
});

app.Run();


public record OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}
