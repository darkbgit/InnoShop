using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Core.Features.CreateProduct;

public class CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateProductCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<long> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(command);

        product.CreatedAt = DateTime.UtcNow;

        if (!await _unitOfWork.Categories.ExistsAsync(product.CategoryId, cancellationToken))
            throw new ValidationException($"Category with id {product.CategoryId} does not exist.");

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
