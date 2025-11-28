using MediatR;
using ProductManagement.Core.DTOs;

namespace ProductManagement.Core.Features.GetCategories;

public class GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>
{

}
