using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Services.OrderService;
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

        [HttpPost("Place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var response = await _orderService.CreateOrderAsync(userId, orderDto);

                if (response.Message == "Insufficient stock")
                    return BadRequest(new { message = response.Message });

                return Ok(new { message = response.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while placing the order.", error = ex.Message });
            }
        }
        
        [HttpGet("my-orders")]
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
    }
}
