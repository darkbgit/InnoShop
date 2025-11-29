using MediatR;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.DeleteProductsByUserId;

public class DeleteProductsByUserIdCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
}
