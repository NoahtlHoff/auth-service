using AuthService.Contracts;
using AuthService.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Services;

public class GuestTokenService(IJwtTokenService tokenService) : IGuestTokenService
{
    public TokenResponse Join(JoinRequest request)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new("name", request.Name),
            new(ClaimTypes.Role, "guest")
        ];

        return new TokenResponse(tokenService.CreateToken(claims, expireHours: 2));
    }
}