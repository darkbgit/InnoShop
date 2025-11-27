using ProductManagement.Core.DTOs;
using ProductManagement.Core.Enums;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using Shared.Core.Models;

namespace ProductManagement.Core.Interfaces;

public interface IProductReadRepository : IReadRepository<Product>
{
    Task<ProductDto?> GetProductDtoByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ProductDto>> GetPaginatedProductDtosAsync(int pageNumber, int pageSize, ProductsSortEnum sortBy, SortOrderEnum sortOrder, string? searchString = null, string? createdBy = null, CancellationToken cancellationToken = default);
}
