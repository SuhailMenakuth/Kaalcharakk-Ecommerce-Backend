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
                .Include(u => u.Role)
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

        //pending 


        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiryDate)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
            }

            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = expiryDate
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }

}

