using Microsoft.AspNetCore.Identity;

namespace UserManagement.Infrastructure.Data;

public class ApplicationRole(string name) : IdentityRole<Guid>(name)
{
}
