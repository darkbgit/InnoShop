using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Domain.Entities;

public class Product : IBaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsOnSale { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid UserId { get; set; }
}
