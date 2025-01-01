using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models.Product;

namespace Kaalcharakk.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly KaalcharakkDbContext _context;

        public ProductRepository(KaalcharakkDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

    }
}
