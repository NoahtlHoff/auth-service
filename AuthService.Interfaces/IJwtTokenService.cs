using System.Security.Claims;

namespace AuthService.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(Claim[] claims, int expireHours);
}
