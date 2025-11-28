namespace UserManagement.Infrastructure.Interfaces;

public interface IProductIntegrationService
{
    Task RestoreProductsForUserAsync(Guid userId);
    Task DeleteProductsForUserAsync(Guid userId);
}
