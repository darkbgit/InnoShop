namespace ProductManagement.Core.Interfaces;

public interface IProductRestoreService
{
    Task RestoreProductsForUserAsync(string userId);
}
