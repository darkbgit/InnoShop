namespace UserManagement.Core.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(Guid userId);

    string GetClaim(string token, string claimType);

    bool ValidateToken(string token);
}
