namespace ShoppingCart.Domain.Entities;

public record CartItem(int Id, string Name, decimal Price, int Quantity = 1);

