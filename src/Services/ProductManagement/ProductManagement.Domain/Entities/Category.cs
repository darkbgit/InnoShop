using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Domain.Entities;

public class Category : IBaseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
