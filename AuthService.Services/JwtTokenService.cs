using AuthService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Services;

public class JwtTokenService(IConfiguration config) : IJwtTokenService
{
    public string CreateToken(Claim[] claims, int expireHours)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(config["Jwt:PrivateKey"]);
        var key = new RsaSecurityKey(rsa);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}