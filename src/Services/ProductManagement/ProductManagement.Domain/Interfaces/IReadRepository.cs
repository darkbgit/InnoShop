using System.Linq.Expressions;

namespace ProductManagement.Domain.Interfaces;

public interface IReadRepository<T> where T : class, IBaseEntity
{
    IQueryable<T> FindByAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll();
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
