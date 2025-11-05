using AutoMapper;
using MediatR;
using ProductManagement.Core.Exceptions;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Core.Features.UpdateProduct;

public class UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var productDb = await _unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken) ??
            throw new NotFoundException($"Product with id {command.Id} doesn't exist.");

        if (command.CategoryId.HasValue && !await _unitOfWork.Categories.ExistsAsync(command.CategoryId.Value, cancellationToken))
            throw new NotFoundException($"Category with id {command.CategoryId.Value} doesn't exist.");

        _mapper.Map(command, productDb);

        _unitOfWork.Products.Update(productDb);
        return await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
