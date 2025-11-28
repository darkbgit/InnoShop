using MediatR;

namespace ProductManagement.Core.Features.CreateProduct;

public class CreateProductCommand : IRequest<long>
{
    public string Name { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsOnSale { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsDeleted { get; set; }
    public long CategoryId { get; set; }
    public Guid CreatedBy { get; set; }
}
