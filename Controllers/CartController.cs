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
                    return StatusCode(202, new ApiResponse<CartResponseDto>(202,"request is succes full but cart not foundd ",  error :"add product to cart first "));
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
            if(respose.StatusCode == 404)
            {
                return StatusCode(404,new ApiResponse<string>(404, " Product not available"));
            }
            if (respose.StatusCode == 403)
            {
                return StatusCode(403, respose);
            }
            if (respose.StatusCode == 409)
            {
                return StatusCode(409, new ApiResponse<string>(422, "insuficient stock "));
            }
            return Ok(respose);
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
           var response = await _cartService.RemoveItemAsync(userId, productId);

            if(response.Message == "cart canot found ")
            {
                return NotFound(response);
            }
            if(response.Message == "product canot found in your cart")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("increase/{productId}")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> IncreaseQuantity( int productId)
        {
            try
            {

            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            var response = await _cartService.UpdateItemQuantityAsync(userId, productId, increase: true);
            if(response.StatusCode == 422)
            {
                StatusCode(422, response);

            }
            if(response.StatusCode == 404)
            {
                return NotFound(response);
            }
            if(response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception($" internal server error : {ex.Message} ");
            }
        }

        
        [HttpPost("decrease/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQuantity( int productId)
        {

            try
            {

            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
           var response =  await _cartService.UpdateItemQuantityAsync(userId, productId, increase: false);
            if (response.StatusCode == 422)
            {
                StatusCode(422, response);

            }
            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception($" internal server error : {ex.Message} ");
            }
        }

        [HttpDelete("clear-cart")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            var response = await _cartService.RemoveAllItemsAsync(userId);
            if(response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            if(response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return Ok(response);

            return Ok(new ApiResponse<string>(200, "All items have been removed from the cart."));
        }
    }

}
