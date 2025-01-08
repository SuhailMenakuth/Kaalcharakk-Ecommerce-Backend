using Kaalcharakk.Configuration;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaalcharakk.Repositories.WishlistRepository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly KaalcharakkDbContext _context;

        public WishlistRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetWishlistByUserIdAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Wishlist> CreateWishlistAsync(int userId)
        {
            var wishlist = new Wishlist { UserId = userId, Items = new List<WishlistItem>() };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task UpdateWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
        }
    }
}
