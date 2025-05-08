using Reporting.Api.Abstractions;
using Reporting.Api.Services;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IProductCatalogService, ApiProductCatalogService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7291");
});

builder.Services.AddHttpClient<IOrderingService, ApiOrderingService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7263");
});

var app = builder.Build();

app.MapGet("/", () => "Hello Reporting.Api!");


app.MapGet("/api/reports/{reportId}", async (string reportId, IProductCatalogService productCatalogService, IOrderingService orderingService) =>
{
    var productsTask = productCatalogService.GetAllAsync();
    var ordersCountTask = orderingService.GetOrdersCountAsync();

    await Task.WhenAll(productsTask, ordersCountTask);

    var products = productsTask.Result;
    var ordersCount = ordersCountTask.Result;

    var salesReport = new SalesReport(reportId, products.Count, ordersCount);

    return Results.Ok(salesReport);

});

app.MapPost("/api/largereport", async () =>
{
    // TODO: Wrzucenie do kolejki
    ConcurrentQueue<SalesReport> reports = new ConcurrentQueue<SalesReport>();
    reports.Enqueue(new SalesReport("ZAM1/2025", 100, 200));

    return Results.Accepted();
});

app.Run();


public record SalesReport(string ReportId, int ProductsCount, int OrdersCount);

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
}
