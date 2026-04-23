using AuthService.Contracts;

namespace AuthService.Interfaces;

public interface IAdminService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request);
    Task<AdminResult> CreateAsync(CreateAdminRequest request);
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
    Task DeleteAsync(int userId);
    Task<List<AdminResult>> GetAllAsync();
}