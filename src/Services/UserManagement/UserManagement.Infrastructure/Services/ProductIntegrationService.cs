using UserManagement.Infrastructure.Interfaces;

namespace UserManagement.Infrastructure.Services;

public class ProductIntegrationService(IHttpClientFactory httpClientFactory) : IProductIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    public async Task DeleteProductsForUserAsync(Guid userId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsync($"/internal/delete-products/{userId}", null);
        
        response.EnsureSuccessStatusCode();
    }

    public async Task RestoreProductsForUserAsync(Guid userId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsync($"/internal/restore-products/{userId}", null);
        
        response.EnsureSuccessStatusCode();
    }
}
