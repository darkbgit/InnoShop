using MediatR;
using ProductManagement.Domain.Interfaces;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.RestoreProductsByUserId;

public class RestoreProductsByUserIdHandler(IUnitOfWork unitOfWork) : IRequestHandler<RestoreProductsByUserIdCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(RestoreProductsByUserIdCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Products.RestoreProductsByUserIdAsync(request.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
