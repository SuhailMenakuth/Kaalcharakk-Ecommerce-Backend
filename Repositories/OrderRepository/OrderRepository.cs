using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaalcharakk.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KaalcharakkDbContext _context;

        public OrderRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
        {
            try
            {

                var userCart = await _context.Carts
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (userCart == null || userCart.Items == null || userCart.Items.Count == 0)
                {

                    throw new Exception("Cart is empty or not found");
                }
                var newOrder = new Order
                {
                    UserId = userId,
                    TotalAmount = createOrderDto.Totalamount,
                    OrderStatus = OrderStatus.Pending,
                    ShippingAddressId = createOrderDto.AddressId,
                    TransactionId = createOrderDto.TransactionId,
                    OrderItems = userCart.Items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price,
                        TotalPrice = item.Product.Price * item.Quantity
                    }).ToList()
                };

                await _context.Orders.AddAsync(newOrder);
                _context.Carts.Remove(userCart);
                await _context.SaveChangesAsync();
                return newOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"error occured when creating order {ex.InnerException}");
            }
        }


        
        public async Task<bool> DecrementStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApiResponse<string>> ValidateCartStockAsync(int userId)
        {
            try
            {

             var userCart = await _context.Carts.Include(x => x.Items).FirstOrDefaultAsync(x => x.UserId == userId);
            if (userCart == null)
            {
                return new ApiResponse<string>(404, "not found", error: "you dont have cart ");

            }
            if (!userCart.Items.Any())
            {
                return new ApiResponse<string>(404, "cart is empty", error: "you dont have any items in your cart");

            }
            //if (userCart == null || userCart.Items == null || userCart.Items.Count == 0)
            //    return false;

            var errorMessages = new List<string>();


            foreach (var item in userCart.Items)
            { 
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (product == null)
                {
                    errorMessages.Add($"Product with ID {item.ProductId} does not exist.");
                    continue;
                }
                if ( product.Stock < item.Quantity )
                {
                    errorMessages.Add($"Insufficient stock for product '{product.Name}'. Available stock: {product.Stock}");
                }
                if (product.IsActive == false)
                {
                    errorMessages.Add($"Product '{product.Name}' is not active.");
                }
                product.Stock -= item.Quantity;
            }

            if (errorMessages.Any())
            {
                return new ApiResponse<string>(400, "Bad request", error: string.Join(" | ", errorMessages));
            }
            return new ApiResponse<string>(200, "success","product validarion successfull");
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error {ex.InnerException}");
            }
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
        //public async Task<Order> CreateOrderAsync(Order order)
        //{
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();
        //    return order;
        //}

        //public async Task<Order> GetOrderAsync(int orderId)
        //{
        //    return await _context.Orders.FindAsync(orderId);
        //}

        //public async Task<List<Order>> GetOrdersAsync()
        //{
        //    return await _context.Orders.ToListAsync();
        //}

        //public async Task<bool> UpdateOrderAsync(Order order)
        //{
        //    _context.Orders.Update(order);
        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> DeleteOrderAsync(int orderId)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);
        //    if (order != null)
        //    {
        //        _context.Orders.Remove(order);
        //        return await _context.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}


        public async Task<Order> GetOrderByOrderId(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;


        }
    }

}
