using AuthService.Contracts;
using AuthService.Interfaces;
using AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Services;
public class AdminService(IUserRepository repository, IJwtTokenService tokenService) : IAdminService
{
    public async Task<TokenResponse?> LoginAsync(LoginRequest request)
    {
        var user = await repository.GetByEmailAsync(request.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        ];

        return new TokenResponse(tokenService.CreateToken(claims, expireHours: 1));
    }

    public async Task<AdminResult> CreateAsync(CreateAdminRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "admin"
        };

        var created = await repository.CreateAsync(user);
        return ToResult(created);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var user = await repository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await repository.UpdateAsync(user);
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await repository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found");

        await repository.DeleteAsync(user);
    }

    public async Task<List<AdminResult>> GetAllAsync()
    {
        var users = await repository.GetAllAsync();
        return users.Select(ToResult).ToList();
    }

    private static AdminResult ToResult(User user) =>
        new(user.Id, user.Email, user.CreatedAt);
}