﻿using Kaalcharakk.Dtos.CartDtos;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrUpdateItem([FromBody] CartItemRequestDto request)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            await _cartService.AddOrUpdateItemAsync(userId, request);
            return NoContent();
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
            return Ok();
        }

        
        [HttpPost("decrease/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQuantity( int productId)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            await _cartService.UpdateItemQuantityAsync(userId, productId, increase: false);
            return Ok();
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
