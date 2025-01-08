using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.WishlistRepository
{
    public interface IWishlistRepository
    {
        Task<Wishlist> GetWishlistByUserIdAsync(int userId);
        Task<Wishlist> CreateWishlistAsync(int userId);
        Task UpdateWishlistAsync(Wishlist wishlist);
    }
}
