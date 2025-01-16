using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        //Task<Order> CreateOrderAsync(Order order);
        //Task<Order> GetOrderAsync(int orderId);
        //Task<List<Order>> GetOrdersAsync();
        //Task<bool> UpdateOrderAsync(Order order);
        //Task<bool> DeleteOrderAsync(int orderId);

        Task<Order> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<bool> DecrementStockAsync(int productId, int quantity);
        Task<bool> ValidateCartStockAsync(int userId);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
    }
}
