namespace AuthService.Contracts;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);