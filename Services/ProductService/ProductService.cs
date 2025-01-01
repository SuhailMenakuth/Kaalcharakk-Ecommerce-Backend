using AutoMapper;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.CloudinaryHelper;
using Kaalcharakk.Models;
using Kaalcharakk.Models.Product;
using Kaalcharakk.Repositories.ProductRepository;

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

        public async Task<bool> AddProductServiceAsync(ProductDto productDto,ProductImageDto productimagedto, ProductSizeListDto productsizelistdto)
        {
            try
            {
                if (productimagedto.Images == null || !productimagedto.Images.Any())
                {
                    throw new ArgumentException("No images provided for upload.", nameof(productimagedto.Images));
                }
                 if(productsizelistdto.ProductSizes == null || !productsizelistdto.ProductSizes.Any())
                { 
                    throw new Exception("ProductSizes in ProductListDto is null or empty.");
                }

                var imageUrls = new List<string>();

                foreach (var imageFile in productimagedto.Images)
                {
                    var imageUrl = await _cloudinaryHelper.UploadProductImageAsyn(imageFile);
                    imageUrls.Add(imageUrl);
                }

                var product = _mapper.Map<Product>(productDto);
                product.ProductImages = imageUrls.Select(url => new ProductImage { ImageUrl = url }).ToList();
                foreach (var size in productsizelistdto.ProductSizes)
                {
                    Console.WriteLine($"Size: {size.Size}, Stock: {size.StockQuantity}");
                }
                product.ProductSizes = productsizelistdto.ProductSizes.Select(size => new ProductSize
                {   
                    Size = size.Size,
                    StockQuantity = size.StockQuantity
                }).ToList();


                //product.ProductImages = imageUrls.Select(url => new ProductImage { ImageUrl = url }).ToList();

                product.StockQuantity = product.ProductSizes.Sum(size => size.StockQuantity);
                var result = await _productRepository.AddProductAsync(product);
                if (result != null)
                {
                    return true;
                }

                return false;

               
               
               
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product", ex);
            }

        }

       
    }
}
