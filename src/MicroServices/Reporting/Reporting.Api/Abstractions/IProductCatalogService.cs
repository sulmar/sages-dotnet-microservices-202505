namespace Reporting.Api.Abstractions;

public interface IProductCatalogService
{
    Task<List<ProductDto>> GetAllAsync();
}

public interface IOrderingService
{
    Task<int> GetOrdersCountAsync();
}

