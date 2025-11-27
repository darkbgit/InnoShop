using MediatR;
using ProductManagement.Core.DTOs;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.GetPaginatedProduct;

public class GetPaginatedProductsQuery : IRequest<PaginatedList<ProductDto>>
{
    public string? SearchString { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public string? CreatedBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
