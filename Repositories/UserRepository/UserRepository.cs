using Kaalcharakk.Configuration;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaalcharakk.Repositories.UserRepository
{

   
    public class UserRepository : IUserRepositoy
    {

        private readonly KaalcharakkDbContext _context;

        public UserRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }
       public async Task<User> FetchUserById(int userid)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Cart)
                .ThenInclude(c => c.Items)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(u => u.UserId == userid);
        }
    }
}
