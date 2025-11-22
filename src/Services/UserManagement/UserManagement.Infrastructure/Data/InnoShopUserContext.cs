using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Data;

public class InnoShopUserContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public InnoShopUserContext(DbContextOptions<InnoShopUserContext> options)
            : base(options)
    {
    }
}
