using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Core.DTOs;

public class ProductForCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Summary { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsOnSale { get; set; }
    public decimal SalePrice { get; set; }
    [Required]
    public long CategoryId { get; set; }
}
