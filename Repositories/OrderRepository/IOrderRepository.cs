using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;

namespace Kaalcharakk.Repositories.OrderRepository
{
    public interface IOrderRepository
    {

        Task<Order> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<bool> DecrementStockAsync(int productId, int quantity);
        Task<ApiResponse<string>> ValidateCartStockAsync(int userId);
        Task<List<Order>> GetOrdersByUserAsync(int userId);

        Task<Order> GetOrderByOrderId(int orderId);
        Task<bool> UpdateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetAllPendingOrdersAsync();
        Task<List<Order>> GetAllProcessingOrdersAsync();
        Task<List<Order>> GetAllDeliveredOrdersAsync();
        Task<List<Order>> GetAllCancelledOrdersAsync();

        Task<List<Order>> GetAllShippedOrdersAsync();




    }
}
