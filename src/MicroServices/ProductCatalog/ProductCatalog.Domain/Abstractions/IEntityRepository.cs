namespace ProductCatalog.Domain.Abstractions;

// Szablon
public interface IEntityRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
}