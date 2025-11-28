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

        var user1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var user2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var user3Id = Guid.Parse("00000000-0000-0000-0000-000000000003");

        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new() { Name = "Smartphone", Summary = "Latest model smartphone", Description = "High-end smartphone with advanced features", Price = 699.99m,  IsAvailable = true, IsOnSale = true, SalePrice = 599.99m, CategoryId = 1, UserId = user1Id },
                new() { Name = "Laptop", Summary = "High performance laptop", Description = "Powerful laptop for work and gaming", Price = 1299.99m, IsAvailable = true, CategoryId = 1, UserId = user1Id },
                new() { Name = "Novel", Summary = "Bestselling fiction novel", Description = "A captivating story that keeps you hooked", Price = 19.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "T-Shirt", Summary = "100% cotton t-shirt", Description = "Comfortable and breathable fabric", Price = 9.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "Jeans", Summary = "Comfortable denim jeans", Description = "Stylish and durable denim jeans", Price = 49.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "E-Reader", Summary = "Portable e-reader device", Description = "Read your favorite books on the go", Price = 129.99m, CategoryId = 1, UserId = user2Id },
                new() { Name = "Science Fiction Book", Summary = "Exciting sci-fi novel", Description = "A thrilling sci-fi adventure", Price = 14.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "Jacket", Summary = "Waterproof outdoor jacket", Description = "Waterproof outdoor jacket", Price = 89.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "Headphones", Summary = "Noise-canceling headphones", Description = "Noise-canceling headphones", Price = 149.99m, CategoryId = 1, UserId = user1Id },
                new() { Name = "Cookbook", Summary = "Delicious recipes from around the world", Description = "Delicious recipes from around the world", Price = 24.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "Sweater", Summary = "Warm and comfortable sweater", Description = "Warm and comfortable sweater", Price = 39.99m, CategoryId = 3, UserId = user3Id },
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
