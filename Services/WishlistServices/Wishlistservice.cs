using Kaalcharakk.Dtos.WishlistDtos;
using Kaalcharakk.Helpers.Response;
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
        public async Task<ApiResponse<WishlistResponseDto>> GetWishlistAsync(int userId)
        {
            try
            {

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) return new ApiResponse<WishlistResponseDto>(404, "not found", error: "you dont have any cart , please add a product to wishlist first"); 

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

            return new ApiResponse<WishlistResponseDto>(200,"success",response);
            }
            catch (Exception ex)
            {
                throw new Exception($"internal exception {ex.Message}", ex);
            }
        }

        // Add item to the wishlist
        public async Task<ApiResponse<string>> AddItemAsync(int userId, int productId)
        {
            try
            {

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null )
                return new ApiResponse<string>(400, "not found", error: "product not found");
             if( product.Stock < 1)
            {
                return new ApiResponse<string>(404, "not found", error: "Product is out of stock");
            }

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId)
                            ?? await _wishlistRepository.CreateWishlistAsync(userId);

            if (wishlist.Items.Any(item => item.ProductId == productId))
                return new ApiResponse<string>(409, "Conflict", error: "Product is already in the wishlist.");

            wishlist.Items.Add(new WishlistItem
            {
                ProductId = productId
            });

            await _wishlistRepository.UpdateWishlistAsync(wishlist);

            return new ApiResponse<string>(200, "success", $"Product added to wishlist {product.Name} ");
            }
            catch(Exception ex)
            {
                throw new Exception($"internal exception {ex.Message}", ex);
            }
        }

        // Move item to cart
        public async Task<ApiResponse<string>> MoveToCartAsync(int userId, int productId)
        {
            try
            {

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);

            if (wishlist == null) return new ApiResponse<string>(400, "BadRequest", error: "no whishlist");

            var wishlistItem = wishlist.Items.FirstOrDefault(item => item.ProductId == productId);
            if (wishlistItem == null) return new ApiResponse<string>(404, "not found", error: "your wishlist is empty");
             


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
            return new ApiResponse<string>(200, "success", $"Product added to cart {productId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"internal exception {ex.Message}", ex);
            }
        }

        // Remove item from the wishlist
        public async Task<ApiResponse<string>> RemoveItemAsync(int userId, int productId)
        {
            try
            {

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);

            if (wishlist == null) return new ApiResponse<string>(400, "BadRequest", error: "you dont have wishlist");
                    

            var product = wishlist.Items.FirstOrDefault(item => item.ProductId == productId);
            if (product == null) return new ApiResponse<string>(404, "not found", error: $"item {productId} not found in your wishlist ");
          

            wishlist.Items.Remove(product);
            await _wishlistRepository.UpdateWishlistAsync(wishlist);

            return new ApiResponse<string>(200, "success", "product removed from the wishlist ");
            }
            catch (Exception ex)
            {
                throw new Exception($"internal exception {ex.Message}", ex);
            }
        }

        // Remove all items from the wishlist
        public async Task<ApiResponse<string>> RemoveAllItemsAsync(int userId)
        {
            try
            {

            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) return new ApiResponse<string>(400, "BadRequest", error: " you dont have wishlist");

            wishlist.Items.Clear();
            await _wishlistRepository.UpdateWishlistAsync(wishlist);
            return new ApiResponse<string>(200, "success", "wishlist cleared successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"internal exception {ex.Message}", ex);
            }
        }
    }
}
 