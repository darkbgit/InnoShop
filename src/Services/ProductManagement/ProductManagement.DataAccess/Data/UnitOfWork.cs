using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.Data;

internal class UnitOfWork(InnoShopContext context, IProductRepository productRepository, IReadRepository<Category> categoryRepository) : IUnitOfWork
{
    private readonly InnoShopContext _context = context;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IReadRepository<Category> _categoryRepository = categoryRepository;

    public IProductRepository Products => _productRepository;
    public IReadRepository<Category> Categories => _categoryRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
