using Kaalcharakk.Dtos.CartDtos;
using Kaalcharakk.Helpers.Response;

namespace Kaalcharakk.Services.CartService
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(int userId);
        Task<ApiResponse<string>> AddOrUpdateItemAsync(int userId, CartItemRequestDto requestDto);
        Task<ApiResponse<string>> RemoveItemAsync(int userId, int productId);
        Task<ApiResponse<string>> UpdateItemQuantityAsync(int userId, int quantity , bool increase);
        Task RemoveAllItemsAsync(int userId);
    }
}
