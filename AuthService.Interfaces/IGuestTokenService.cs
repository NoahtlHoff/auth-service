using AuthService.Contracts;

namespace AuthService.Interfaces;

public interface IGuestTokenService
{
    TokenResponse Join(JoinRequest request);
}