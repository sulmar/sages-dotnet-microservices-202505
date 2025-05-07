using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Intrastructure;

public class RedisCartItemRepository : ICartItemRepository
{
    public Task AddAsync(CartItem item)
    {
        // Implementation for adding item to Redis
        throw new NotImplementedException();
    }
}
