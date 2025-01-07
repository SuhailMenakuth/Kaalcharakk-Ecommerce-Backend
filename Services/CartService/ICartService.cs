using Kaalcharakk.Dtos.CartDtos;

namespace Kaalcharakk.Services.CartService
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(int userId);
        Task AddOrUpdateItemAsync(int userId, CartItemRequestDto requestDto);
        Task RemoveItemAsync(int userId, int productId);
        Task UpdateItemQuantityAsync(int userId, int quantity , bool increase);
        Task RemoveAllItemsAsync(int userId);
    }
}
