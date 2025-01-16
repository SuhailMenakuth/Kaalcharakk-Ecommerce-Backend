using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.UserRepository
{
    public interface IUserRepositoy
    {
        Task<List<User>> FetchAllUserAsync();
        Task<User> FetchUserByIdAsync(int id);
        Task<List<User>> FetchAllBlockedUsersAsync();
        Task<List<User>> FetchAllUnBlockedUsersAsync();
        Task UpdateUserAsync(User user);


    }
}
