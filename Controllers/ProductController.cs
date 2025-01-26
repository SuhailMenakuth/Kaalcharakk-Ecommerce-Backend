using CloudinaryDotNet;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
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

        [HttpPost("addproduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto addproductDto, [FromForm] AddProductImageDto addproductimagedto)
        {
            var response = await _productService.AddProductServiceAsync(addproductDto, addproductimagedto);

            if (response.StatusCode == 503)
            {
                return StatusCode(503, response);
            }
            if (response.StatusCode == 501)
            {
                return StatusCode(501, response);
            }

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }

            return StatusCode(500, response);



        }

        [HttpGet("product/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var response = await _productService.GetProductByIdServiceAsync(id);
                if (response.StatusCode == 400)
                {
                    return NotFound(response);
                }
                if (response.StatusCode == 500)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("allproducts/admin-view")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var response = await _productService.GetAllProductsServiceAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }

        }
        

        [HttpGet("allactive/products")]
        
        public async Task<IActionResult> GetAllActiveProducts()
        {
            try
            {
                var response = await _productService.GetAllProductsForUsersServiceAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
              //  return StatusCode(500, new { ex.Message });
              return StatusCode(500, ex);
            }

        }




        [HttpGet("filter-products")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery]ProductFilterDto filterCriteria)
        {
            try
            {
                var response = await _productService.GetProductsByFilterServiceAsync(filterCriteria);

                if(response.StatusCode == 204)
                {
                    return StatusCode(204, response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch("activate-product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateProductById(int id)
        {
            try
            {
                var response = await _productService.ActivaeOrDeactivateProductByIdServiceAsync(id, activate: true);
                if (response.StatusCode == 200)
                {
                    return Ok(response);

                }
                if (response.StatusCode == 404)
                {
                    return BadRequest(response);
                }

                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPatch("deactivate-product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateProductById(int id )
        {
            try
            {
                var response = await _productService.ActivaeOrDeactivateProductByIdServiceAsync(id, activate : false);
                if (response.StatusCode == 200)
                {
                    return Ok(response);

                }
                if (response.StatusCode == 404)
                {
                    return BadRequest(response);
                }

                return StatusCode(500, response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }





        [HttpPatch("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductAsync(int productId,[FromForm] UpdateProductDto updateProductDto,IFormFile? newImage = null)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(new ApiResponse<string>(400, "Invalid request data"));
                //}

                var result = await _productService.UpdateProductServiceAsync(productId, updateProductDto, newImage);

                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                else if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }
                else
                {
                    return StatusCode(500, result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "An error occurred", error: ex.Message));
            }
        }

    }
}