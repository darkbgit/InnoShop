using MediatR;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Interfaces;

namespace ProductManagement.Core.Features.GetCategories;

public class GetCategoriesHandler(ICategoryReadRepository repository) : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryReadRepository _repository = repository;

    public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetCategoriesDtoAsync();

        return result;
    }
}