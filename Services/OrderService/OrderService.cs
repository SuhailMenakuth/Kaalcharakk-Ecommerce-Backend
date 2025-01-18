using AutoMapper;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.RazorPayHelper;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.OrderRepository;
using Kaalcharakk.Services.AddressService;

namespace Kaalcharakk.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRazorpayHelper _razorpayHelper;
        private readonly IMapper _mapper;
        private readonly IAddressService _addressService; 


        public OrderService(IOrderRepository orderRepository, IRazorpayHelper razorpayHelper,IMapper mapper , IAddressService addressService)
        {
            _orderRepository = orderRepository;
            _razorpayHelper = razorpayHelper;
            _mapper = mapper;
            _addressService = addressService;
            
        }

        public async Task<ApiResponse<string>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto )
        {
            try
            {



                if (createOrderDto == null)
                {
                    return new ApiResponse<string>(400, "CreateOrderDto cannot be null");
                }

                var userAdress = await _addressService.GetShippingAddressesAsync(userId);
                if (!userAdress.Data.Any())
                {
                    return new ApiResponse<string>(404, "Address canot be empty", error: "you dont have any adress");
                }

                if (!await _orderRepository.ValidateCartStockAsync(userId))
                return new ApiResponse<string>(400,"Insufficient stock");

           
            var razorpayOrderId = await _razorpayHelper.CreateRazorpayOrder((long)createOrderDto.Totalamount);

           
            createOrderDto.TransactionId = razorpayOrderId;
            var order = await _orderRepository.CreateOrderAsync(userId, createOrderDto);

            return new ApiResponse<string>(200,"Order created successfully", razorpayOrderId);
            }
            catch( Exception ex)
            {
                throw;
            }
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

        public async Task<ApiResponse<OrderViewDto>> GetOrderByOrderById(int orderId)
        {
            var res = await _orderRepository.GetOrderByOrderId(orderId);

            if(res == null)
            {
                return new ApiResponse<OrderViewDto>(404,"order canot found ");
            }

            //var order = _mapper.Map<OrderViewDto>(res);

            var orderView = new OrderViewDto
            {
                TransactionId = res.TransactionId,
                TotalAmount = res.TotalAmount,
                OrderStatus = res.OrderStatus.ToString(),
                DeliveryAdrress = res.ShippingAddress.Address,
                Phone = res.ShippingAddress?.Phone ?? "No phone number provided",
                OrderDate = res.OrderDate,
                Items = res.OrderItems?.Select(oi => new OrderItemDto
                {
                    //ProductName = oi.Product?.Name 
                    //?? "Unknown Product", 
                    ProductName = oi.Product.Name,

                    Quantity = oi.Quantity,
                    Price = oi.Price * oi.Quantity,
                }).ToList() 
                ?? new List<OrderItemDto>()
            };

            return new ApiResponse<OrderViewDto>(200, "success", orderView);

        }

        public async Task<ApiResponse<string>> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                return new ApiResponse<string>(400, "Invalid order status value");
            }
            var res = await _orderRepository.GetOrderByOrderId(orderId);

            if (res == null)
            {
                return new ApiResponse<string>(404, "order canot found ");
            }

            res.OrderStatus = status;

            var isUpdated = await _orderRepository.UpdateOrderAsync(res);
            if (!isUpdated)
            {
                return new ApiResponse<string>(500, "internal server error ", error:"error occured when updating the order status");
            }
            
            return new ApiResponse<string>(200, "success","order Updated succesfully");

        }
    }
}
