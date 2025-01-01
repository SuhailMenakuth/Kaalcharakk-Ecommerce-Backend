using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models.Product;

namespace Kaalcharakk.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
    }
}
