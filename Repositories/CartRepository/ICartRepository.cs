using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.CartRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync(int userId);
        Task UpdateCartAsync(Cart cart);

    }
}
