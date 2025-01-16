using Kaalcharakk.Dtos.CartDtos;
using Kaalcharakk.Helpers.Response;

namespace Kaalcharakk.Services.CartService
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(int userId);
        Task<ApiResponse<string>> AddOrUpdateItemAsync(int userId, CartItemRequestDto requestDto);
        Task RemoveItemAsync(int userId, int productId);
        Task UpdateItemQuantityAsync(int userId, int quantity , bool increase);
        Task RemoveAllItemsAsync(int userId);
    }
}
