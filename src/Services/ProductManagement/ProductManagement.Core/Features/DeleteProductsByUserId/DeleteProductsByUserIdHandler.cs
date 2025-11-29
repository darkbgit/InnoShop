using MediatR;
using ProductManagement.Domain.Interfaces;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.DeleteProductsByUserId;

public class DeleteProductsByUserIdHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductsByUserIdCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(DeleteProductsByUserIdCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Products.DeleteProductsByUserIdAsync(request.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
