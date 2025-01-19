using AutoMapper;
using CloudinaryDotNet;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.CloudinaryHelper;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.ProductRepository;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection.Metadata.Ecma335;

namespace Kaalcharakk.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryHelper _cloudinaryHelper;

        public ProductService(IProductRepository productRepository , IMapper mapper , ICloudinaryHelper cloudinaryHelper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cloudinaryHelper = cloudinaryHelper;
        }

        public async Task<ApiResponse<string>> AddProductServiceAsync(AddProductDto addproductdto,AddProductImageDto addproductimagedto)
        {
            try
            {
                
                var imageUrl = await _cloudinaryHelper.UploadProductImageAsyn(addproductimagedto.Image);

                if(imageUrl == null)
                {
                    return new ApiResponse<string>(503, "internal server error  ",error:"internal server error cloudinary updation error ");
                }
                var product = _mapper.Map<Product>(addproductdto);
                if(product == null)
                {
                    return new ApiResponse<string>(501, "internal server error", error: "Maping of your product details to actual product details ");
                }
                product.ImageUrl = imageUrl;
                var result = await _productRepository.AddProductAsync(product);
                if (result != null)
                {  
                return new ApiResponse<string>(200,"success" ,error: "Producted added succesfully");   
                }

                    return new ApiResponse<string>(500, "internal server error  ", error: "error occured during the updation of the database");
               
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product", ex);
            }

        }

      public async  Task<ApiResponse<ProductViewDto>> GetProductByIdServiceAsync(int id)
        {
            try
            {
                var RetrivedProduct = await _productRepository.GetProductByIdAsync(id);

                if (RetrivedProduct == null)
                {

                    return new ApiResponse<ProductViewDto>(400, "error",error: "Cant find a product with this id");

                }
                //var result = _mapper.Map<ProductViewDto>(RetrivedProduct);
                //if (result == null)
                //{
                //    return new ApiResponse<ProductViewDto>(500, "internal server error", error: "retrived product mapping failed ");
                //}
                var result = new ProductViewDto
                {
                    ProductId = RetrivedProduct.ProductId,
                    Category = RetrivedProduct.Category.Name,
                    Name = RetrivedProduct.Name,
                    Price = RetrivedProduct.Price,
                    ImageUrl = RetrivedProduct.ImageUrl,
                    Color = RetrivedProduct.Color,
                    Stock = RetrivedProduct.Stock,
                    Offer = RetrivedProduct.Offer,
                    OfferStartingDate = RetrivedProduct.OfferStartingDate,
                    OfferEndingDate = RetrivedProduct.OfferEndingDate,
                    IsActive = RetrivedProduct.IsActive
                };

                return new ApiResponse<ProductViewDto>(200, "success",result);
            }
            catch (Exception ex)
            {
                throw;
                //new Exception("An error occurred while fetching the product", ex);
            }
        }

        public async Task<List<ProductViewDto>> GetAllProductsServiceAsync()
        {
            try
            {
                //List<ProductViewDto> products = new List<ProductViewDto>();

                 var products  = await _productRepository.GetAllProductsAsync();

                var AllProductList = products.Select(p =>

                new ProductViewDto
                {

                    ProductId = p.ProductId,
                    Category = p.Category.Name,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Color = p.Color,
                    Stock = p.Stock,
                    Offer = p.Offer,
                    OfferStartingDate = p.OfferStartingDate,
                    OfferEndingDate = p.OfferEndingDate,
                    IsActive = p.IsActive
                    

                } ).ToList();

                return AllProductList;

             
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ApiResponse<List<ProductViewDto>>> GetAllProductsForUsersServiceAsync()
        {
            try
            {
                var products = await _productRepository.GetAllProductsUsers();
                var AllProductList =  products.Select(p =>

                new ProductViewDto
                {

                    ProductId = p.ProductId,
                    Category = p.Category.Name,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Color = p.Color,
                    Stock = p.Stock,
                    Offer = p.Offer,
                    OfferStartingDate = p.OfferStartingDate,
                    OfferEndingDate = p.OfferEndingDate,
                    IsActive = p.IsActive


                }).ToList();

                return  new ApiResponse<List<ProductViewDto>>(200,"success", AllProductList);
            }
            catch (Exception ex)
            {
                throw new Exception($"internal server error  , {ex.Message}");
            }
        }

        public async Task< ApiResponse< List<ProductViewDto>>> GetProductsByFilterServiceAsync(ProductFilterDto filterDto)
        {
            try
            {
                

                var products = await _productRepository.GetProductsByFilterAsync(filterDto);

                var filteredProducts = products.Select(product => new ProductViewDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Category = product.Category.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Color = product.Color,
                    Offer = product.Offer,
                    OfferStartingDate = product.OfferStartingDate,
                    OfferEndingDate = product.OfferEndingDate,
                    IsActive = product.IsActive
                }).ToList();

                if (filteredProducts.Count < 1)
                {
                    return new ApiResponse<List<ProductViewDto>>(204, "no content");
                }
                return new ApiResponse<List<ProductViewDto>>(200, "success", filteredProducts);
            }
            catch (Exception ex) {
                throw new Exception($"internal server erro{ex.InnerException}");
                    }

        }
        


        public async Task<ApiResponse<string>> ActivaeOrDeactivateProductByIdServiceAsync(int id, bool activate)
        {
           var product = await _productRepository.GetProductByIdAsync(id);
            if(product == null)
            {
                return new ApiResponse<string>(404, "product not found");

            }

            product.IsActive = activate;

            var updatedProduct = await _productRepository.UpdateProductAsync(product);

            if (!updatedProduct)
            {
                return new ApiResponse<string>(500, "internal server error", error: "error occured when updating the product");
            }

            return new ApiResponse<string>(200, "success", "product updated successfully");


        }


        public async Task<ApiResponse<string>> UpdateProductServiceAsync(int productId, UpdateProductDto updateProductDto, IFormFile newImage = null)
        {
            try
            {
                
                var existingProduct = await _productRepository.GetProductByIdAsync(productId);
                if (existingProduct == null)
                {
                    return new ApiResponse<string>(404, "Product not found");
                }

               
                if (newImage != null)
                {
                    
                    var existingImagePublicId = _cloudinaryHelper.ExtractPublicIdFromUrl(existingProduct.ImageUrl);

                   
                    await _cloudinaryHelper.DeleteImageAsync(existingImagePublicId);

                    var newImageUrl = await _cloudinaryHelper.UploadProductImageAsyn(newImage);
                    existingProduct.ImageUrl = newImageUrl;
                }

                
                existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
                existingProduct.Price = updateProductDto.Price ?? existingProduct.Price;
                existingProduct.CategoryId = updateProductDto.CategoryId ?? existingProduct.CategoryId;
                existingProduct.Color = updateProductDto.Color ?? existingProduct.Color; 
                existingProduct.Stock = updateProductDto.Stock ?? existingProduct.Stock;
                existingProduct.Offer = updateProductDto.Offer ?? existingProduct.Offer;
                existingProduct.OfferStartingDate = updateProductDto.OfferStartingDate ?? DateTime.UtcNow;
                existingProduct.OfferEndingDate = updateProductDto.OfferEndingDate ?? DateTime.UtcNow;
                existingProduct.IsActive = updateProductDto.IsActive ?? existingProduct.IsActive;

                
                var updateResult = await _productRepository.UpdateProductAsync(existingProduct);
                if (updateResult)
                {
                    return new ApiResponse<string>(200,"success", $"Product updated successfully {productId}");
                }

                return new ApiResponse<string>(500,"internal server error", error:$"Failed to update the product");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product", ex);
            }
        }


    }
}
