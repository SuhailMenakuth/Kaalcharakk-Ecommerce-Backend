using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaalcharakk.Repositories.AdressRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly KaalcharakkDbContext _context;

        public AddressRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }



        public async Task<bool> CreateAddressAsync(ShippingAddress shippingadrress)
        {
            _context.ShippingAddresses.Add(shippingadrress);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserWithShippingAddressesAsync(int userId)
        {
            return await _context.Users.Include(x => x.ShippingAddresses).FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<bool> RemoveShippingAddressAsync(User user, ShippingAddress address)
        {
            user.ShippingAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<int> GetShippingAdressCount(int UId)
        {
           return await _context.ShippingAddresses.CountAsync(x => x.UserId == UId);
        }
    }
}
