using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Interfaces;

public interface IUnitOfWork
{
    IReadRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
