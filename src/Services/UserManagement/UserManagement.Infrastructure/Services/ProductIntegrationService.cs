using System;
using UserManagement.Infrastructure.Interfaces;

namespace UserManagement.Infrastructure.Services;

public class ProductIntegrationService : IProductIntegrationService
{
    public Task DeleteProductsForUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task RestoreProductsForUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}
