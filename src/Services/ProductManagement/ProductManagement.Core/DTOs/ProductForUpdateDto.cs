namespace ProductManagement.Core.DTOs;

public class ProductForUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsOnSale { get; set; }
    public decimal? SalePrice { get; set; }
    public long? CategoryId { get; set; }
}
