using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;

namespace Kaalcharakk.Services.AddressService
{
    public interface IAddressService
    {
        Task<ApiResponse<int>> CreateShippingAddress(int userUId, OrderAddressDto orderAddressDTO);

        Task<ApiResponse<List<ViewAddressDto>>> GetShippingAddressesAsync(int userId);

        Task<bool> RemoveShippingAddressByUserAsync(int userId, int addressId);

    }
}
