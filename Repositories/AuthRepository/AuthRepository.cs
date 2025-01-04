using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.AuthRepository;
using Microsoft.EntityFrameworkCore;

namespace Kaalcharakk.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly KaalcharakkDbContext _context;

        public AuthRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                . Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> GetUserByPhoneAsync(string phone)
        {
            var IsExist = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (IsExist == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            Console.WriteLine($"Before saving: IsActived = {user.IsActived}");
            await _context.SaveChangesAsync();
        }

     
    }
}
