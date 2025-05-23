﻿using Kaalcharakk.Dtos.CartDtos;
using Kaalcharakk.Helpers.Response;
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
           Console.WriteLine(cart);
            if (cart == null) return null;

            var response = new CartResponseDto
            {
                CartId = cart.CartId,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                      Category = item.Product.Category.Name,
                    ImageUrl = item.Product.ImageUrl,


                }).ToList()
            };

            response.TotalPrice = response.Items.Sum(item => item.Total);
            return response;
        }

        public async Task<ApiResponse<string>> AddOrUpdateItemAsync(int userId, int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null 
                //requestDto.Quantity
                )
                return new ApiResponse<string>(404,"", error:" Product not available");
            if(!product.IsActive)
                {
                    return new ApiResponse<string>(403,"", error:"the product is inactive and cannot be added to the cart");
                }
                if(product.Stock < 1)
                {
                return new ApiResponse<string>(422,"", "insuficient stock ");
            };

            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? await _cartRepository.CreateCartAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                    

                if (cartItem.Quantity > product.Stock)

                    return new ApiResponse<string>(422, "unprocessable Entity", error: "insufficient stock");
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId ,
                    Quantity = 1
                });
            }

            await _cartRepository.UpdateCartAsync(cart);
            return new ApiResponse<string>(200,"success", $"product added sucessfully {productId}  ");

        }

        public async Task<ApiResponse<string>> RemoveItemAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {

                return new ApiResponse<string>(404, "not found", error:"cart canot found ");
            }

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null)
            {
                return new ApiResponse<string>(404, "not found", error:"product canot found in your cart ");
            }

            cart.Items.Remove(cartItem);
            await _cartRepository.UpdateCartAsync(cart);
            return new ApiResponse<string>(200, "success",$"item removed from the cart successfully {productId}");
        }

        public async Task<ApiResponse<string>> UpdateItemQuantityAsync(int userId, int productId, bool increase)
        {
            try
            {

            var product = await _productRepository.GetProductByIdAsync(productId);
            if ( product.Stock < 1)
                return new ApiResponse<string>(422, "unprocessable Entity", error: "insufficient stock");

            if (product == null)
            {
                return new ApiResponse<string>(404, "not found", error: "product not found");
            }

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ApiResponse<string>(404,"not found" ,error:"cart not found for this user");
            }

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem == null)
            {
                return new ApiResponse<string>(404,"not found",error:"Item not found in cart.");
            }

            
            if (increase)
            {
                cartItem.Quantity += 1;

                if (cartItem.Quantity > product.Stock)
                    return new ApiResponse<string>(422, "unprocessable Entity", error: "insufficient stock");
            }
            
            else
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    return new ApiResponse<string>(400, "Badrequest", error: "Quantity cannot be less than 1");
                }
            }

            await _cartRepository.UpdateCartAsync(cart);
            return new ApiResponse<string>(200, "success", $"Quantity Updated Successfully {product.Name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error{ ex.InnerException }");
            }
        }

        public async Task<ApiResponse<string>> RemoveAllItemsAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            
            if (cart == null)
            {
                return new ApiResponse<string>(404, "not found", error: $"cart not found for this ${userId}user");
            }
            if (cart.Items.Count < 1)
            {
                return new ApiResponse<string>(400, "Badrequest", error: "you cart is empty");
            }

            cart.Items.Clear();
            await _cartRepository.UpdateCartAsync(cart);
            return new ApiResponse<string>(200, "success", $"cart cleared success fully  Successfully");


        }
    }
}