using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenService(IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        var key = configuration["Jwt:Key"] ??
            throw new ArgumentNullException("Jwt key is not configured");

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(Guid userId)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, userId.ToString()),
            };

        var applicationUser = await _userManager.FindByIdAsync(userId.ToString()) ??
            throw new InvalidOperationException("User not found");

        var roles = await _userManager.GetRolesAsync(applicationUser);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler
            .CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GetClaim(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.ReadToken(token) is not JwtSecurityToken securityToken)
        {
            return string.Empty;
        }

        var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
        return stringClaimValue;
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validateParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = _key,
            ValidateLifetime = true,
        };

        try
        {
            tokenHandler.ValidateToken(token, validateParameters, out var validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
