using ProductCatalog.Api.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Mappers;

public static class ProductMapper
{
    public static ProductDto MapToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            DiscountedPrice = product.DiscountedPrice
        };
    }
    public static Product MapToEntity(this ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Price = productDto.Price,
            DiscountedPrice = productDto.DiscountedPrice
        };
    }
}
