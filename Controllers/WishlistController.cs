using Kaalcharakk.Services.WishlistServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("get/mywishlist")]
        [Authorize]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {

            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            var resposnse = await _wishlistService.GetWishlistAsync(userId);
            return Ok(resposnse);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Message : {ex.Message}  Error : {ex.InnerException}" );
            }
        }

        [HttpPost("add/to-wishlist/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
               var response = await _wishlistService.AddItemAsync(userId, productId);
               if(response.StatusCode == 409)
                {
                    return StatusCode(409, response);
                }

               if (response.StatusCode == 404)
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
                return StatusCode(500, $"Message : {ex.Message}  Error : {ex.InnerException}"); ;
            }
        }



        [HttpPost("movetocart/{productId}")]
        [Authorize]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
                var  response =  await _wishlistService.MoveToCartAsync(userId, productId);
                if(response.StatusCode == 400)
                {
                    return StatusCode(400, response);
                }
                if(response.StatusCode == 404)
                {
                    return StatusCode(404, response);
                }
                return Ok("Product moved to cart.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Message : {ex.Message}  Error : {ex.InnerException}");
            }
        }

        [HttpDelete(" delete/product/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
               var response = await _wishlistService.RemoveItemAsync(userId, productId);
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
                return StatusCode(500, $"Message : {ex.Message}  Error : {ex.InnerException}");
            }
        }


        [HttpDelete("clear/wishlist")]
        [Authorize]
        public async Task<IActionResult> ClearWishlist()
        {
            
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
               var response = await _wishlistService.RemoveAllItemsAsync(userId);
                if(response.StatusCode == 400)
                {
                    return BadRequest(400);
                }
               
                return Ok("Wishlist cleared.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Message : {ex.Message}  Error : {ex.InnerException}");
            }
        }
    }
}
