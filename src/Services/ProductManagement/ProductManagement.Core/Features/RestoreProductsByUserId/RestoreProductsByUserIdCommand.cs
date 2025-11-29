using MediatR;
using Shared.Core.Models;

namespace ProductManagement.Core.Features.RestoreProductsByUserId;

public class RestoreProductsByUserIdCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
}
