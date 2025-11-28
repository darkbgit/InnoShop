using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Interfaces;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;

namespace ProductManagement.DataAccess.Repositories;

internal class CategoryReadRepository(InnoShopContext context, IMapper mapper) : BaseReadRepository<Category>(context), ICategoryReadRepository
{
    private readonly IMapper _mapper = mapper;

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesDtoAsync(CancellationToken cancellationToken = default)
    {
        var categories = await GetAll().ToListAsync(cancellationToken);

        var result = categories.Select(_mapper.Map<CategoryDto>).ToList();

        return result;
    }
}
