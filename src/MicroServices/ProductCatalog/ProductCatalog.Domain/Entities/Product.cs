namespace ProductCatalog.Domain.Entities;

public abstract class Base
{

}

public abstract class BaseEntity : Base
{
    public int Id { get; set; }

    protected BaseEntity(int id)
    {
        Id = id;
    }

    protected BaseEntity()
    {
        
    }
}

public class Product : BaseEntity    
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }

    public Product() : base()
    {
        
    }

    public Product(int id, string name, decimal price, decimal? discountedPrice = null) : base(id)
    {
        Name = name;
        Price = price;
        DiscountedPrice = discountedPrice;
    }
}

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Category(int id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }
}
