using Reporting.Api.Abstractions;

namespace Reporting.Api.Services;

public class ApiProductCatalogService(HttpClient _httpClient) : IProductCatalogService
{
    public Task<List<ProductDto>> GetAllAsync() => _httpClient.GetFromJsonAsync<List<ProductDto>>("/api/products");
}

public class ApiOrderingService : IOrderingService
{
    private readonly HttpClient _httpClient;

    public ApiOrderingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetOrdersCountAsync()
    {
        var orderingResponse = await _httpClient.GetAsync("/api/orders/count");
        var ordersCount = await orderingResponse.Content.ReadAsStringAsync();
        return int.Parse(ordersCount);
    }
}
