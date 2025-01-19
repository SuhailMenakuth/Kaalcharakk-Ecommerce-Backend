using Kaalcharakk.Dtos.WishlistDtos;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.CartRepository;
using Kaalcharakk.Repositories.ProductRepository;
using Kaalcharakk.Repositories.WishlistRepository;

namespace Kaalcharakk.Services.WishlistServices
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public WishlistService(IWishlistRepository wishlistRepository, ICartRepository cartRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        // Get wishlist for the user
        public async Task<WishlistResponseDto> GetWishlistAsync(int userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) return null;

            var response = new WishlistResponseDto
            {
                WishlistId = wishlist.WishlistId,
                Items = wishlist.Items.Select(item => new WishlistItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    ImageUrl = item.Product.ImageUrl,
                }).ToList()
            };

            return response;
        }

        // Add item to the wishlist
        public async Task AddItemAsync(int userId, int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.Stock < 1)
                throw new Exception("Product not available or insufficient stock.");

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId)
                            ?? await _wishlistRepository.CreateWishlistAsync(userId);

            if (wishlist.Items.Any(item => item.ProductId == productId))
                throw new Exception("Product already in wishlist.");

            wishlist.Items.Add(new WishlistItem
            {
                ProductId = productId
            });

            await _wishlistRepository.UpdateWishlistAsync(wishlist);
        }

        // Move item to cart
        public async Task MoveToCartAsync(int userId, int productId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) throw new Exception("Wishlist not found.");

            var wishlistItem = wishlist.Items.FirstOrDefault(item => item.ProductId == productId);
            if (wishlistItem == null) throw new Exception("Item not found in wishlist.");

            // Remove from wishlist
            //wishlist.Items.Remove(wishlistItem);
            //await _wishlistRepository.UpdateWishlistAsync(wishlist);

            // Add to cart
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                       ?? await _cartRepository.CreateCartAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1
                });
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        // Remove item from the wishlist
        public async Task RemoveItemAsync(int userId, int productId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) throw new Exception("Wishlist not found.");

            var wishlistItem = wishlist.Items.FirstOrDefault(item => item.ProductId == productId);
            if (wishlistItem == null) throw new Exception("Item not found in wishlist.");

            wishlist.Items.Remove(wishlistItem);
            await _wishlistRepository.UpdateWishlistAsync(wishlist);
        }

        // Remove all items from the wishlist
        public async Task RemoveAllItemsAsync(int userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) throw new Exception("Wishlist not found.");

            wishlist.Items.Clear();
            await _wishlistRepository.UpdateWishlistAsync(wishlist);
        }
    }
}
 