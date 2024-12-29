using Kaalcharakk.Models;

namespace Kaalcharakk.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }
}
