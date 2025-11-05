using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.Data;

internal class UnitOfWork(InnoShopContext context, IRepository<Product> productRepository, IReadRepository<Category> categoryRepository) : IUnitOfWork
{
    private readonly InnoShopContext _context = context;
    private readonly IRepository<Product> _productRepository = productRepository;
    private readonly IReadRepository<Category> _categoryRepository = categoryRepository;

    public IRepository<Product> Products => _productRepository;
    public IReadRepository<Category> Categories => _categoryRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
