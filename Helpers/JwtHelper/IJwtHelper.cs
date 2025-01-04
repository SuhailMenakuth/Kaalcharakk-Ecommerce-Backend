using Kaalcharakk.Models;

namespace Kaalcharakk.Helpers.JwtHelper.JwtHelper
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    
    }
}
