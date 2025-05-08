var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ProductCatalog", client =>
{
    client.BaseAddress = new Uri("https://localhost:7291");
});

builder.Services.AddHttpClient("Ordering", client =>
{
    client.BaseAddress = new Uri("https://localhost:7263");
});

var app = builder.Build();

app.MapGet("/", () => "Hello Reporting.Api!");


app.MapGet("/api/reports/{reportId}", async (string reportId, IHttpClientFactory factory) =>
{
    var productCatalogClient = factory.CreateClient("ProductCatalog");
    var orderingClient = factory.CreateClient("Ordering");

    var productCatalogResponse = await productCatalogClient.GetAsync("/api/products");
    var products = await productCatalogResponse.Content.ReadFromJsonAsync<List<ProductDto>>();

    var orderingResponse = await orderingClient.GetAsync("/api/orders/count");
    var ordersCount = await orderingResponse.Content.ReadAsStringAsync();

    var salesReport = new SalesReport(reportId, products.Count, int.Parse(ordersCount));

    return Results.Ok(salesReport);

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
