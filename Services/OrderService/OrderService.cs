using AutoMapper;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.RazorPayHelper;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.CartRepository;
using Kaalcharakk.Repositories.OrderRepository;
using Kaalcharakk.Repositories.ProductRepository;
using Kaalcharakk.Services.AddressService;
using Razorpay.Api;
using System.Runtime.CompilerServices;

namespace Kaalcharakk.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRazorpayHelper _razorpayHelper;
        private readonly IMapper _mapper;
        private readonly IAddressService _addressService;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;


        public OrderService(IOrderRepository orderRepository, IRazorpayHelper razorpayHelper,IMapper mapper , IAddressService addressService , ICartRepository cartRepository , IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _razorpayHelper = razorpayHelper;
            _mapper = mapper;
            _addressService = addressService;
            _productRepository = productRepository;
            
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
                    return new ApiResponse<string>(404, "not found", error: "Address canot be empty");
                }


                var response = await _orderRepository.ValidateCartStockAsync(userId);
                if (response.StatusCode == 400) return response;
                if (response.StatusCode == 404) return response;
               
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
            var order = await _orderRepository.GetOrderByOrderId(orderId);

            if (order == null)
            {
                return new ApiResponse<string>(404, "order canot found ");
            }

            order.OrderStatus = status;


            var isUpdated = await _orderRepository.UpdateOrderAsync(order);
            if (!isUpdated)
            {
                return new ApiResponse<string>(500, "internal server error ", error:"error occured when updating the order status");
            }


            if(status == OrderStatus.Cancelled)
            {
                foreach(var orderItem in order.OrderItems)
                {
                    var product = await _productRepository.GetProductByIdAsync(orderItem.ProductId);
                    if(product != null)
                    {
                        product.Stock += orderItem.Quantity;
                        var isProductUpdated = await _productRepository.UpdateProductAsync(product);

                        if (!isProductUpdated)
                        {
                            return new ApiResponse<string>(500, "Internal server error", error: $"Failed to update stock for product ID {orderItem.ProductId}");
                        }
                    }
                }
            }

            
         
            
            return new ApiResponse<string>(200, "success","order Updated succesfully");

        }


        public async Task<ApiResponse<List<OrderViewDto>>> GetAllOrderServiceAsync()
        {

            try
            {

            
            var orders = await _orderRepository.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return new ApiResponse<List<OrderViewDto>>(200, "Success." , error:"There is no orders currenetly" );
            }

            var orderViewDtos = orders.Select(order => new OrderViewDto
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                OrderDate = order.OrderDate,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductName = oi.Product?.Name ?? "Unknown Product",
                    Quantity = oi.Quantity,
                    Price = oi.Price * oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error , {ex.InnerException}");
            }

           
        }

         public  async Task<ApiResponse<List<OrderViewDto>>> GetAllPendingOrdersServiceAsync()
         {

            try
            {

              var PendingOrders = await _orderRepository.GetAllPendingOrdersAsync();
            if (PendingOrders == null || !PendingOrders.Any())
            {
                return new ApiResponse<List<OrderViewDto>>(200, "success" , error:"no pending orders currently");
            }
            var orderViewDtos = PendingOrders.Select(order => new OrderViewDto
            {
                TransactionId = order.TransactionId,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                OrderDate = order.OrderDate,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductName = oi.Product?.Name ?? "Unknown Product",
                    Quantity = oi.Quantity,
                    Price = oi.Price * oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();
            return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error , Message : {ex.Message} , InnerException : {ex.InnerException}");
            }


        }

        public async Task<ApiResponse<List<OrderViewDto>>> GetAllProcessingOrdersServiceAsync()
        {
            try
            {

                var ProcessingOrders = await _orderRepository.GetAllPendingOrdersAsync();
                if (ProcessingOrders == null || !ProcessingOrders.Any())
                {
                    return new ApiResponse<List<OrderViewDto>>(200, "success", error: "no pending orders currently");
                }
                var orderViewDtos = ProcessingOrders.Select(order => new OrderViewDto
                {
                    TransactionId = order.TransactionId,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus.ToString(),
                    DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                    Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                    OrderDate = order.OrderDate,
                    Items = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product?.Name ?? "Unknown Product",
                        Quantity = oi.Quantity,
                        Price = oi.Price * oi.Quantity,
                        TotalPrice = oi.TotalPrice,


                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList();
                return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw;
                    //new Exception($"Service error: {ex.Message}", ex); 
            }
        }


       public async Task<ApiResponse<List<OrderViewDto>>> GetAllDeliveredOrdersServiceAsync()
        {
            try
            {

                var DeliveredOrders = await _orderRepository.GetAllDeliveredOrdersAsync();
                if (DeliveredOrders == null || !DeliveredOrders.Any())
                {
                    return new ApiResponse<List<OrderViewDto>>(200, "success", error: "no pending orders currently");
                }
                var orderViewDtos = DeliveredOrders.Select(order => new OrderViewDto
                {
                    TransactionId = order.TransactionId,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus.ToString(),
                    DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                    Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                    OrderDate = order.OrderDate,
                    Items = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product?.Name ?? "Unknown Product",
                        Quantity = oi.Quantity,
                        Price = oi.Price * oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList();
                return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw;
                //new Exception($"Service error: {ex.Message}", ex); 
            }
        }

        public async Task<ApiResponse<List<OrderViewDto>>> GetAllCancelledOrdersServiceAsync()
        {
            try
            {

                var CancelledOrders = await _orderRepository.GetAllCancelledOrdersAsync();
                if (CancelledOrders == null || !CancelledOrders.Any())
                {
                    return new ApiResponse<List<OrderViewDto>>(200, "success", error: "no pending orders currently");
                }
                var orderViewDtos = CancelledOrders.Select(order => new OrderViewDto
                {
                    TransactionId = order.TransactionId,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus.ToString(),
                    DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                    Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                    OrderDate = order.OrderDate,
                    Items = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product?.Name ?? "Unknown Product",
                        Quantity = oi.Quantity,
                        Price = oi.Price * oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList();
                return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw;
                //new Exception($"Service error: {ex.Message}", ex); 
            }
        }


       public async Task<ApiResponse<List<OrderViewDto>>> GetAllShippedOrdersServiceAsync()
        {
            try
            {

                var ShippedOrders = await _orderRepository.GetAllShippedOrdersAsync();
                if (ShippedOrders == null || !ShippedOrders.Any())
                {
                    return new ApiResponse<List<OrderViewDto>>(200, "success", error: "no pending orders currently");
                }
                var orderViewDtos = ShippedOrders.Select(order => new OrderViewDto
                {
                    TransactionId = order.TransactionId,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus.ToString(),
                    DeliveryAdrress = order.ShippingAddress?.Address ?? "No address provided",
                    Phone = order.ShippingAddress?.Phone ?? "No phone number provided",
                    OrderDate = order.OrderDate,
                    Items = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product?.Name ?? "Unknown Product",
                        Quantity = oi.Quantity,
                        Price = oi.Price * oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList();
                return new ApiResponse<List<OrderViewDto>>(200, "Orders retrieved successfully.", orderViewDtos);
            }
            catch (Exception ex)
            {
                throw;
                //new Exception($"Service error: {ex.Message}", ex); 
            }
        }



    }
}
