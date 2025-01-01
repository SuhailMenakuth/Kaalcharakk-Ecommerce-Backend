using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models.Product;

namespace Kaalcharakk.Services.ProductService
{
    public interface IProductService
    {
        Task<bool> AddProductServiceAsync(ProductDto productdto , ProductImageDto productimagedto ,ProductSizeListDto productsizelistdto);
    }
}
