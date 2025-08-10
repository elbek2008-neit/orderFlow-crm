using CrmOrderManagement.Core.AuthEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserClaims userClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
        UserClaims? GetUserClaimsFromToken(string token);
    }
}
