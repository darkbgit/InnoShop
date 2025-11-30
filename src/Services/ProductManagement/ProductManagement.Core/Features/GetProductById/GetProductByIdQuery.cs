using MediatR;
using ProductManagement.Core.DTOs;

namespace ProductManagement.Core.Features.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDetailDto>
{
    public long Id { get; set; }
}
