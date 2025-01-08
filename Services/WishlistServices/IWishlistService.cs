using Kaalcharakk.Dtos.WishlistDtos;
using Kaalcharakk.Models;

namespace Kaalcharakk.Services.WishlistServices
{
    public interface IWishlistService
    {
        //Task<WishlistResponseDto> GetWishlistAsync(int userId);
        ////Task AddOrUpdateItemAsync(int userId, WishlistItemRequestDto requestDto);
        //Task RemoveItemAsync(int userId, int productId);
        //Task RemoveAllItemsAsync(int userId);


        Task<WishlistResponseDto> GetWishlistAsync(int userId);
        Task AddItemAsync(int userId, int productId);
        Task MoveToCartAsync(int userId, int productId);
        Task RemoveItemAsync(int userId, int productId);
        Task RemoveAllItemsAsync(int userId);


        //Task<Wishlist> GetWishlistByUserIdAsync(int userId);
        //Task<Wishlist> CreateWishlistAsync(int userId);
        //Task UpdateWishlistAsync(Wishlist wishlist);
    }
}
