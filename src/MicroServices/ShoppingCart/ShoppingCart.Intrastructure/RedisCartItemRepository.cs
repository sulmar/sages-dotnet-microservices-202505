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
        // TODO: Pobierz sessionId z HttpContext.Session
        string sessionId = "001abc";

        string key = $"cart:{sessionId}";
        string field = $"product:{item.Id}";

        // Zwiększ ilość produktu w koszyku
        await db.HashIncrementAsync(key, field, item.Quantity);

        // Ustaw TTL dla klucza (np. 3 minuty)
        await db.KeyExpireAsync(key, TimeSpan.FromMinutes(3));        
    }
}
