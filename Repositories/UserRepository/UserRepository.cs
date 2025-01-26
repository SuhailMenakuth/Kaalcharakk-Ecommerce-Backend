using Kaalcharakk.Configuration;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace Kaalcharakk.Repositories.UserRepository
{

   
    public class UserRepository : IUserRepositoy
    {

        private readonly KaalcharakkDbContext _context;

        public UserRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }
       public async Task<User> FetchUserByIdAsync(int userid)
        {
            try
            {

            return await _context.Users
                //.Include(u => u.Role)
                //.Include(u => u.ShippingAddresses)// changed 
                //.Include(u => u.Orders)
                //.ThenInclude(o => o.OrderItems)
                //.ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(u => u.UserId == userid);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
           
        }

        public async Task<List<User>> FetchAllUserAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "User")
                .ToListAsync();
        }

        public async Task<List<User>> FetchAllUnBlockedUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName =="Users" && u.IsActived == true)
                .ToListAsync();
        }
        public async Task<List<User>> FetchAllBlockedUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName =="Users" && u.IsActived == false)
                .ToListAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }



    }
}
