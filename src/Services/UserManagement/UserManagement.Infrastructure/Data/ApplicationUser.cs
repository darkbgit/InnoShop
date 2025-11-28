using Microsoft.AspNetCore.Identity;

namespace UserManagement.Infrastructure.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public bool IsDeleted { get; set; }
}
