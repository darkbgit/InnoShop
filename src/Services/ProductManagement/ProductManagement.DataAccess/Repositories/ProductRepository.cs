using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.Repositories;

internal class ProductRepository(InnoShopContext context) : BaseRepository<Product>(context), IProductRepository
{
    public Task DeleteProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            throw new ArgumentException("Invalid userId format. Must be a valid GUID.", nameof(userId));
        }
        
        var products = Table
            .Where(p => p.UserId == userGuid && !p.IsDeleted);

        foreach (var product in products)
        {
            product.IsDeleted = true;
        }

        return Task.CompletedTask;
    }

    public Task RestoreProductsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            throw new ArgumentException("Invalid userId format. Must be a valid GUID.", nameof(userId));
        }

        var products = Table
            .IgnoreQueryFilters()
            .Where(p => p.UserId == userGuid && p.IsDeleted);

        foreach (var product in products)
        {
            product.IsDeleted = false;
        }

        return Task.CompletedTask;
    }
}
