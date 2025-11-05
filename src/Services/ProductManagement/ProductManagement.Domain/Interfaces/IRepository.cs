namespace ProductManagement.Domain.Interfaces;

public interface IRepository<T> where T : class, IBaseEntity
{
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}