using Kaalcharakk.Dtos.WishlistDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;

namespace Kaalcharakk.Services.WishlistServices
{
    public interface IWishlistService
    {
        //Task<WishlistResponseDto> GetWishlistAsync(int userId);
        ////Task AddOrUpdateItemAsync(int userId, WishlistItemRequestDto requestDto);
        //Task RemoveItemAsync(int userId, int productId);
        //Task RemoveAllItemsAsync(int userId);


        Task<ApiResponse<WishlistResponseDto>> GetWishlistAsync(int userId);
        Task<ApiResponse<string>> AddItemAsync(int userId, int productId);
        Task<ApiResponse<string>> MoveToCartAsync(int userId, int productId);
        Task<ApiResponse<string>> RemoveItemAsync(int userId, int productId);
        Task<ApiResponse<string>> RemoveAllItemsAsync(int userId);


        //Task<Wishlist> GetWishlistByUserIdAsync(int userId);
        //Task<Wishlist> CreateWishlistAsync(int userId);
        //Task UpdateWishlistAsync(Wishlist wishlist);
    }
}
