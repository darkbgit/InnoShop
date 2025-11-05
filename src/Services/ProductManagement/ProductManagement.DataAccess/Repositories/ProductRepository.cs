using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;

namespace ProductManagement.DataAccess.Repositories;

internal class ProductRepository(InnoShopContext context) : BaseRepository<Product>(context)
{

}
