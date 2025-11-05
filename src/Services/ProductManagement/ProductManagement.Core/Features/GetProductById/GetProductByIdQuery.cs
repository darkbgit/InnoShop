using MediatR;
using ProductManagement.Core.DTOs;

namespace ProductManagement.Core.Features.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public long Id { get; set; }
}
