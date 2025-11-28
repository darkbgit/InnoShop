using ProductManagement.Core.DTOs;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Core.Interfaces;

public interface ICategoryReadRepository : IReadRepository<Category>
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesDtoAsync(CancellationToken cancellationToken = default);
}
