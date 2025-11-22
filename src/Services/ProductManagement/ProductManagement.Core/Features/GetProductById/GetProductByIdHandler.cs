using MediatR;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Interfaces;
using Shared.Core.Exceptions;

namespace ProductManagement.Core.Features.GetProductById;

public class GetProductByIdHandler(IProductReadRepository repository)
     : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductReadRepository _repository = repository;

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductDtoByIdAsync(request.Id, cancellationToken) ??
            throw new NotFoundException($"Product with id {request.Id} not found.");

        return product;
    }
}
