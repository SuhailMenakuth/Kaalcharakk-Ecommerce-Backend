using Kaalcharakk.Dtos.CartDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("my-cart")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            try
            {
                var cart = await _cartService.GetCartAsync(userId);
                if (cart == null)
                {
                    //return StatusCode(204,new ApiResponse<object>(204, "no content"));
                    return NotFound();
                }

                return Ok(new ApiResponse<CartResponseDto>(200, "Cart retrieved successfully", cart));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "An error occurred while retrieving the cart", error: ex.Message));
            }
        }


        [HttpPost("addorupdate")]
        [Authorize]
        public async Task<IActionResult> AddOrUpdateItem([FromBody] CartItemRequestDto request)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var respose =await _cartService.AddOrUpdateItemAsync(userId, request);
            if(respose.Message == "insuficient stock or Product not available")
            {
                return StatusCode(409,new ApiResponse<string>(409, "insuficient stock or Product not available"));
            }
            if (respose.Message == "insuficient stock")
            {
                return StatusCode(409, new ApiResponse<string>(409, "insuficient stock "));
            }
            return Ok("Cart Updated");
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            await _cartService.RemoveItemAsync(userId, productId);
            return NoContent();
        }

        [HttpPost("increase/{productId}")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> IncreaseQuantity( int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            await _cartService.UpdateItemQuantityAsync(userId, productId, increase: true);
            return Ok("Quantity Updated");
        }

        
        [HttpPost("decrease/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQuantity( int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            await _cartService.UpdateItemQuantityAsync(userId, productId, increase: false);
            return Ok("Quantity Updated");
        }

        [HttpDelete("clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            await _cartService.RemoveAllItemsAsync(userId);
            return Ok("All items have been removed from the cart.");
        }
    }

}
