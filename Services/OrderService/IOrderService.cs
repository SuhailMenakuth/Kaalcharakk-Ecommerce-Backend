﻿using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;

namespace Kaalcharakk.Services.OrderService
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<List<OrderViewDto>> GetOrdersAsync(int userId);
        Task<ApiResponse<OrderViewDto>> GetOrderByOrderById(int orderId);
        Task<ApiResponse<string>> UpdateOrderStatus(int orderId, OrderStatus status);
        Task<ApiResponse<List<OrderViewDto>>> GetAllOrderServiceAsync();
        Task<ApiResponse<List<OrderViewDto>>> GetAllPendingOrdersServiceAsync();
        Task<ApiResponse<List<OrderViewDto>>> GetAllProcessingOrdersServiceAsync();
        Task<ApiResponse<List<OrderViewDto>>> GetAllDeliveredOrdersServiceAsync();
        Task<ApiResponse<List<OrderViewDto>>> GetAllCancelledOrdersServiceAsync();
        Task<ApiResponse<List<OrderViewDto>>> GetAllShippedOrdersServiceAsync();
    }


}
