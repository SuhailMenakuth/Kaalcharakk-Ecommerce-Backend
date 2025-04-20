using Kaalcharakk.Models;
using System.Security.Claims;

namespace Kaalcharakk.Helpers.JwtHelper.JwtHelper
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    }
}
