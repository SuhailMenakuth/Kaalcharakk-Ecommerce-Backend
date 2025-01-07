using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.UserRepository
{
    public interface IUserRepositoy
    {
        Task<User> FetchUserById(int id);
    }
}
