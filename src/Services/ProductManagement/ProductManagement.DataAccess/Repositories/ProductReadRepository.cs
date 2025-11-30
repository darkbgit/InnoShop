using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Enums;
using ProductManagement.Core.Interfaces;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;
using Shared.Core.Models;

namespace ProductManagement.DataAccess.Repositories;

internal class ProductReadRepository(InnoShopContext context, IMapper mapper) : BaseReadRepository<Product>(context), IProductReadRepository
{
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<ProductDto>> GetPaginatedProductDtosAsync(int pageNumber, int pageSize, ProductsSortEnum sortBy, SortOrderEnum sortOrder, string? searchString = null, string? createdBy = null, CancellationToken cancellationToken = default)
    {
        var query = Table.AsNoTracking()
            .Include(t => t.Category)
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(products => products.Name.Contains(searchString) ||
                                               products.Description.Contains(searchString) ||
                                               products.Category.Name.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(createdBy) && Guid.TryParse(createdBy, out var guid))
        {
            query = query.Where(p => p.UserId == guid);
        }

        long count = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, sortBy, sortOrder);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => _mapper.Map<ProductDto>(p))
            .ToListAsync(cancellationToken);


        var result = new PaginatedList<ProductDto>(items,
            count,
            pageNumber,
            pageSize);

        return result;
    }

    public Task<ProductDetailDto?> GetProductDtoByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return Table.AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Category)
            .Select(p => _mapper.Map<ProductDetailDto>(p))
            .SingleOrDefaultAsync(cancellationToken);
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductsSortEnum sort, SortOrderEnum order)
    {
        var sortQuery = query.OrderByDescending(p => p.IsAvailable);
        query = sort switch
        {
            ProductsSortEnum.Name when order == SortOrderEnum.Ascending => sortQuery.ThenBy(q => q.Name),
            ProductsSortEnum.Name when order == SortOrderEnum.Descending => sortQuery.ThenByDescending(q => q.Name),
            ProductsSortEnum.Price when order == SortOrderEnum.Ascending => sortQuery.ThenBy(c => c.Price),
            ProductsSortEnum.Price when order == SortOrderEnum.Descending => sortQuery.ThenByDescending(c => c.Price),
            ProductsSortEnum.CreatedDate when order == SortOrderEnum.Ascending => sortQuery.ThenBy(c => c.CreatedAt),
            ProductsSortEnum.CreatedDate when order == SortOrderEnum.Descending => sortQuery.ThenByDescending(c => c.CreatedAt),
            _ => query.OrderBy(c => c.Name)
        };

        return query;
    }
}
