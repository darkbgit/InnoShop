using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;

namespace ProductManagement.DataAccess.Repositories;

internal class CategoryReadRepository(InnoShopContext context) : BaseReadRepository<Category>(context)
{

}
