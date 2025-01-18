using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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


        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
               .Include(p => p.Category)
               .FirstOrDefaultAsync(pi => pi.ProductId == id);
        }

        public async Task<List<Product>> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Category)
                 .Where(p => p.Name.Contains(name))
                 .ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategoryAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == name)
                .ToListAsync();
        }

       public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }



        public async Task<List<Product>> GetProductsByFilterAsync(ProductFilterDto filterDto)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (filterDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filterDto.MinPrice.Value);
            }

            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterDto.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(filterDto.Color))
            {
                query = query.Where(p => p.Color == filterDto.Color);
            }
            if (!string.IsNullOrEmpty(filterDto.Category))
            {
                query = query.Where(p => p.Category.Name == filterDto.Category);
            }

            

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
           return await _context.SaveChangesAsync() > 0 ;
           

        }
        public async Task<List<Product>> GetAllProductsUsers()
        {
            return await _context.Products
                 .Include(p => p.Category)
                 .Where(p => p.IsActive == true)
                 .ToListAsync();
        }

    }
   
}
