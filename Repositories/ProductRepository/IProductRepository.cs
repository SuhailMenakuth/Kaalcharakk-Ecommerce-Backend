
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models;


namespace Kaalcharakk.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id );

        Task<List<Product>> GetProductByNameAsync(string name);

        Task<List<Product>> GetProductByCategoryAsync(string name);

        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetProductsByFilterAsync(ProductFilterDto filterDto);

        Task<bool> UpdateProductAsync(Product product);
        Task<List<Product>> GetAllProductsUsers();

    }
}
