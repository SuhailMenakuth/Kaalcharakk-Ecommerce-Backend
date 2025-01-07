using Kaalcharakk.Dtos.CartDtos;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.CartRepository;
using Kaalcharakk.Repositories.ProductRepository;

namespace Kaalcharakk.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartResponseDto> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return null;

            var response = new CartResponseDto
            {
                CartId = cart.CartId,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            response.TotalPrice = response.Items.Sum(item => item.Total);
            return response;
        }

        public async Task AddOrUpdateItemAsync(int userId, CartItemRequestDto requestDto)
        {
            var product = await _productRepository.GetProductByIdAsync(requestDto.ProductId);
            if (product == null || product.Stock < 1
                //requestDto.Quantity
                )
                throw new Exception("Product not available or insufficient stock.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? await _cartRepository.CreateCartAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == requestDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1
                    //requestDto.Quantity
                    ;

                if (cartItem.Quantity > product.Stock)
                    throw new Exception("Insufficient stock.");
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = requestDto.ProductId,
                    Quantity = 1
                    //requestDto.Quantity
                });
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            cart.Items.Remove(cartItem);
            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task UpdateItemQuantityAsync(int userId, int productId, bool increase)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.Stock < 1)
                throw new Exception("Product not available or insufficient stock.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            // If increasing, just add 1 to the quantity
            if (increase)
            {
                cartItem.Quantity += 1;

                if (cartItem.Quantity > product.Stock)
                    throw new Exception("Insufficient stock.");
            }
            // If decreasing, reduce the quantity by 1 (ensure it doesn't go below 1)
            else
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    throw new Exception("Quantity cannot be less than 1.");
                }
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task RemoveAllItemsAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            // Remove all items from the cart
            cart.Items.Clear();

            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}