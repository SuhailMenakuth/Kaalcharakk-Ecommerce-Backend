using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.AdressRepository
{
    public interface IAddressRepository
    {
        Task<int> GetShippingAdressCount(int UserId);
        Task<bool> CreateAddressAsync(ShippingAddress shippingadrress);
        Task<User?> GetUserWithShippingAddressesAsync(int userId);
        Task<bool> RemoveShippingAddressAsync(User user, ShippingAddress address);

    }
}
