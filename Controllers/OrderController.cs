using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Models;
using Kaalcharakk.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Place/order")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var response = await _orderService.CreateOrderAsync(userId, orderDto);

                if (response.StatusCode == 400)
                    return BadRequest(response);
                if (response.StatusCode == 404)
                {
                    return StatusCode(404, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while placing the order.", error = ex.Message });
                //var errorResponse = new
                //{
                //    message = "An error occurred while processing the order.",
                //    error = ex.Message,  // Include the exception message
                //    stackTrace = ex.StackTrace  // Optionally include the stack trace for debugging
                //};
               // return StatusCode(500, errorResponse);
            }
        }



        [HttpGet("my/orders")]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            try
            {


                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var orders = await _orderService.GetOrdersAsync(userId);

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the orders.", error = ex.Message });
            }
        }


        //[HttpPatch("update-orderstatus")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateOrderStatus(int orderId , OrderStatus orderstatus)
        //{

        //}

        [HttpGet("retrive/order/{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {

            try
            {

                var res = await _orderService.GetOrderByOrderById(orderId);
               
                
                if (res.StatusCode == 404)
                {
                    return BadRequest(res);
                }
                    return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });

            }

        }

        [HttpPatch("update/orders/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId , OrderStatus orderStatus)
        {

            try
            {

                var res = await _orderService.UpdateOrderStatus(orderId, orderStatus);


                if (res.StatusCode == 400)
                {
                    return BadRequest(res);
                }
                if (res.StatusCode == 404)
                {
                    return BadRequest(res);
                }
                if (res.StatusCode == 500)
                {
                    return StatusCode(500, res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpGet("get/all/orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {

           var response = await _orderService.GetAllOrderServiceAsync();
            return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get/all/PendingOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPendingOrders()
        {
            try
            {
                var response = await _orderService.GetAllPendingOrdersServiceAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } 
        
        
        [HttpGet("get/all/ProcessingOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProcessingOrders()
        {
            try
            {
                var response = await _orderService.GetAllProcessingOrdersServiceAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Server Error",
                    message = ex.Message 
                });
            }
        }
        [HttpGet("get/all/shipped-Orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShippedOrders()
        {
            try
            {
                var response = await _orderService.GetAllShippedOrdersServiceAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Server Error",
                    message = ex.Message 
                });
            }
        } 
        
        
        [HttpGet("get/all/Cancelled-orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCancelledOrders()
        {
            try
            {
                var response = await _orderService.GetAllCancelledOrdersServiceAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Server Error",
                    message = ex.Message 
                });
            }
        }
        [HttpGet("get/all-delivered-orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeliveredOrders()
        {
            try
            {
                var response = await _orderService.GetAllDeliveredOrdersServiceAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Server Error",
                    message = ex.Message 
                });
            }
        }








    }
}
