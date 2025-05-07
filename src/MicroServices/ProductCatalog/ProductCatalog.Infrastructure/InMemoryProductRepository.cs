using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure;


public class Context
{
    public IDictionary<int, Product> Products { get; set; } = new Dictionary<int, Product>();
}


public class InMemoryProductRepository : IProductRepository
{
    private readonly Context _context;

    public InMemoryProductRepository(Context context)
    {
        _context = context;
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_context.Products.Values.AsEnumerable());
    }
}

public class DbProductRepository : IProductRepository
{
    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
