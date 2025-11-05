using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;

namespace ProductManagement.DataAccess.Helpers;

public static class SeedDatabase
{
    public static async Task SeedAsync(InnoShopContext context)
    {
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
                    {
                        new() { Name = "Electronics", Description = "Electronic gadgets and devices" },
                        new() { Name = "Books", Description = "Various kinds of books" },
                        new() { Name = "Clothing", Description = "Apparel and accessories" }
                    };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new() { Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, CategoryId = 1 },
                new() { Name = "Laptop", Description = "High performance laptop", Price = 1299.99m, CategoryId = 1 },
                new() { Name = "Novel", Description = "Bestselling fiction novel", Price = 19.99m, CategoryId = 2 },
                new() { Name = "T-Shirt", Description = "100% cotton t-shirt", Price = 9.99m, CategoryId = 3 }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
