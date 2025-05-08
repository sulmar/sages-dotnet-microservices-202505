using ProductCatalog.Api.DTOs;
using ProductCatalog.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace ProductCatalog.Api.Mappers;

// dotnet add package Riok.Mapperly

[Mapper]
public partial class ProductMapper
{
    public partial ProductDto MapToDto(Product product);
    public partial Product MapToEntity(ProductDto productDto);
}
