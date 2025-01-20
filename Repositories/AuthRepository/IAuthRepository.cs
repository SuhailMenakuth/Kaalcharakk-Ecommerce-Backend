using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User userr);
        Task<bool> GetUserByPhoneAsync(string phone);

        // pending

        Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiryDate);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByIdAsync(int userId);




    }
}
