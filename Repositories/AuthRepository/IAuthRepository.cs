using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User userr);
        Task<bool> GetUserByPhoneAsync(string phone);





    }
}
