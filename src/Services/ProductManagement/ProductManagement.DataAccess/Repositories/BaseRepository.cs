using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.Repositories;

internal class BaseRepository<T>(InnoShopContext context) : IRepository<T> where T : class, IBaseEntity
{
    protected readonly DbSet<T> Table = context.Set<T>();

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return Table.AnyAsync(item => item.Id == id, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await Table.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
    }

    public void Remove(T entity)
    {
        Table.Remove(entity);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }
}
