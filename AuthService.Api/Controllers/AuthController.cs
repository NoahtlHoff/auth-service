using AuthService.Contracts;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAdminService adminService, IGuestTokenService guestTokenService) : ControllerBase
{
    [HttpPost("join")]
    public IActionResult Join(JoinRequest request) =>
        Ok(guestTokenService.Join(request));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await adminService.LoginAsync(request);
        return result is null ? Unauthorized() : Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("admins")]
    public async Task<IActionResult> CreateAdmin(CreateAdminRequest request) =>
        Ok(await adminService.CreateAsync(request));

    [Authorize(Roles = "admin")]
    [HttpPut("admins/password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await adminService.ChangePasswordAsync(userId, request);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("admins")]
    public async Task<IActionResult> Delete()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await adminService.DeleteAsync(userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("admins")]
    public async Task<IActionResult> GetAll() =>
        Ok(await adminService.GetAllAsync());
}