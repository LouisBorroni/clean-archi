namespace TierList.Application.Ports.Services;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email, string username, bool isAdmin);
    bool ValidateToken(string token);
}
