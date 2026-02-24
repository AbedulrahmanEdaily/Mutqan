using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface ITokenService:IScopedService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
