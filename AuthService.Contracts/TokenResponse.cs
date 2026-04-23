namespace AuthService.Contracts;

public record TokenResponse(string Token, string? RefreshToken = null);