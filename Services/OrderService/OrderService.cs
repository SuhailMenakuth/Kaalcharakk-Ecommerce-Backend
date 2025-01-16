using AutoMapper;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.RazorPayHelper;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.OrderRepository;

namespace Kaalcharakk.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRazorpayHelper _razorpayHelper;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepository orderRepository, IRazorpayHelper razorpayHelper,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _razorpayHelper = razorpayHelper;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
        {
         
            if (!await _orderRepository.ValidateCartStockAsync(userId))
                return new ApiResponse<string>(400,"Insufficient stock");

           
            var razorpayOrderId = await _razorpayHelper.CreateRazorpayOrder((long)createOrderDto.Totalamount);

           
            createOrderDto.TransactionId = razorpayOrderId;
            var order = await _orderRepository.CreateOrderAsync(userId, createOrderDto);

            return new ApiResponse<string>(200,"Order created successfully", razorpayOrderId);
        }

        public async Task<List<OrderViewDto>> GetOrdersAsync(int userId)
        {
            var orders  = await _orderRepository.GetOrdersByUserAsync(userId);
            var orderViewDtos = orders.Select(order => new OrderViewDto
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.OrderItems.Sum(item => item.Quantity * item.Price),
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryAdrress = order.ShippingAddress?.FullName,
                Phone = order.ShippingAddress?.Phone,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice * item.Quantity,
                }).ToList()
            }).ToList();

            //return _mapper.Map<List<OrderViewDto>>(orders);
            return orderViewDtos;

        }
    }
}
