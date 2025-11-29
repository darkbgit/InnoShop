using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task RestoreProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task DeleteProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
