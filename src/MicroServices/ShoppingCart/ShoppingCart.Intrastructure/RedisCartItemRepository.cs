using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using StackExchange.Redis;

namespace ShoppingCart.Intrastructure;

// dotnet add package StackExchange.Redis
public class RedisCartItemRepository : ICartItemRepository
{
    private readonly IDatabase db;

    public RedisCartItemRepository(IDatabase db)
    {
        this.db = db;
    }

    public async Task AddAsync(CartItem item)
    {
        string sessionId = "001abc";

        string key = $"cart:{sessionId}";
        string field = $"product:{item.Id}";

        await db.HashIncrementAsync(key, field, item.Quantity);
        // await db.StringGetSetExpiryAsync(key, TimeSpan.FromSeconds(30));
    }
}
