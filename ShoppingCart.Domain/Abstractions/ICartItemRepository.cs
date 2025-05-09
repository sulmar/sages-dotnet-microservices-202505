using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.Abstractions;

public interface ICartItemRepository
{
    Task AddAsync(string sessionId, CartItem item);
}
