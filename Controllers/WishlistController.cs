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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWishlist()
        {
            
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            var wishlist = await _wishlistService.GetWishlistAsync(userId);
            if (wishlist == null)
                return NotFound("Wishlist not found.");
            return Ok(wishlist);
        }

        [HttpPost("{productId}")]
        [Authorize]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
                await _wishlistService.AddItemAsync(userId, productId);
                return Ok("Product added to wishlist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("move-to-cart/{productId}")]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
                await _wishlistService.MoveToCartAsync(userId, productId);
                return Ok("Product moved to cart.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
                await _wishlistService.RemoveItemAsync(userId, productId);
                return Ok("Product removed from wishlist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> ClearWishlist()
        {
            
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            try
            {
                await _wishlistService.RemoveAllItemsAsync(userId);
                return Ok("Wishlist cleared.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
