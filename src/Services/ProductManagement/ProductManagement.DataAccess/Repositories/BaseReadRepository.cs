using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.Repositories;

internal class BaseReadRepository<T>(InnoShopContext context) : IReadRepository<T> where T : class, IBaseEntity
{
    protected readonly DbSet<T> Table = context.Set<T>();

    public IQueryable<T> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = Table.AsNoTracking().Where(predicate);
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }

    public Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return Table.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public IQueryable<T> GetAll()
    {
        return Table.AsNoTracking();
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return Table.AsNoTracking().AnyAsync(e => e.Id == id, cancellationToken);
    }
}
