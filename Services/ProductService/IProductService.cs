using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;

namespace Kaalcharakk.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponse<string>> AddProductServiceAsync(AddProductDto productdto , AddProductImageDto productimagedto );

        Task<ApiResponse<ProductViewDto>> GetProductByIdServiceAsync(int id);

        Task<List<ProductViewDto>> GetAllProductsServiceAsync();

        Task<List<ProductViewDto>> GetProductsByFilterServiceAsync(ProductFilterDto filterDto);

        Task <ApiResponse<string>> ActivaeOrDeactivateProductByIdServiceAsync(int id , bool activate);
        Task<ApiResponse<string>> UpdateProductServiceAsync(int productId, UpdateProductDto updateProductDto, IFormFile newImage = null);



    }
}
