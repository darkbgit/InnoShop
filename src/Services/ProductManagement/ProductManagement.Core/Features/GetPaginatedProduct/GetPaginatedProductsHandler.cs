using MediatR;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Enums;
using ProductManagement.Core.Interfaces;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.GetPaginatedProduct;

public class GetPaginatedProductsHandler(IProductReadRepository repository)
    : IRequestHandler<GetPaginatedProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IProductReadRepository _repository = repository;

    public async Task<PaginatedList<ProductDto>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.SortOrder, true, out SortOrderEnum sortOrderEnum))
        {
            sortOrderEnum = SortOrderEnum.Ascending;
        }

        if (!Enum.TryParse(request.SortBy, true, out ProductsSortEnum sortByEnum))
        {
            sortByEnum = ProductsSortEnum.Name;
        }

        var paginatedList = await _repository.GetPaginatedProductDtosAsync(
            request.PageNumber,
            request.PageSize,
            sortByEnum,
            sortOrderEnum,
            request.SearchString,
            request.CreatedBy,
            cancellationToken);

        return paginatedList;
    }
}
