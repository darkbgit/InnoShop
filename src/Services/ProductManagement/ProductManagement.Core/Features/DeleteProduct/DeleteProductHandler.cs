using MediatR;
using ProductManagement.Core.Exceptions;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Core.Features.DeleteProduct;

public class DeleteProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products
            .GetByIdAsync(command.Id, cancellationToken) ??
                throw new NotFoundException($"Product with id {command.Id} not found.");

        _unitOfWork.Products.Remove(product);

        return await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
