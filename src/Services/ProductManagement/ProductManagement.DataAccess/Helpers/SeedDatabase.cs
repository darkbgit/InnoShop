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
                new() { Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, CategoryId = 1, UserId = user1Id },
                new() { Name = "Laptop", Description = "High performance laptop", Price = 1299.99m, CategoryId = 1, UserId = user1Id },
                new() { Name = "Novel", Description = "Bestselling fiction novel", Price = 19.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "T-Shirt", Description = "100% cotton t-shirt", Price = 9.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "Jeans", Description = "Comfortable denim jeans", Price = 49.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "E-Reader", Description = "Portable e-reader device", Price = 129.99m, CategoryId = 1, UserId = user2Id },
                new() { Name = "Science Fiction Book", Description = "A thrilling sci-fi adventure", Price = 14.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "Jacket", Description = "Waterproof outdoor jacket", Price = 89.99m, CategoryId = 3, UserId = user3Id },
                new() { Name = "Headphones", Description = "Noise-canceling headphones", Price = 149.99m, CategoryId = 1, UserId = user1Id },
                new() { Name = "Cookbook", Description = "Delicious recipes from around the world", Price = 24.99m, CategoryId = 2, UserId = user2Id },
                new() { Name = "Sweater", Description = "Warm and comfortable sweater", Price = 39.99m, CategoryId = 3, UserId = user3Id },

            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
