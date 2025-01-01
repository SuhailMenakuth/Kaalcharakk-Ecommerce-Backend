using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto productDto , [FromForm] ProductImageDto productImagedto , [FromForm] ProductSizeListDto productsizelistdto)
        {
            var result = await _productService.AddProductServiceAsync(productDto ,productImagedto , productsizelistdto);
            return Ok(result);
        }


    }
}
