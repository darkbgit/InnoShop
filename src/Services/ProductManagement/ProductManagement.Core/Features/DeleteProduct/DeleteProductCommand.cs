using MediatR;

namespace ProductManagement.Core.Features.DeleteProduct;

public class DeleteProductCommand : IRequest<int>
{
    public long Id { get; set; }
}
