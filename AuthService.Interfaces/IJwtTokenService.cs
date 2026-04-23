using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AuthService.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(Claim[] claims, int expireHours);
}
