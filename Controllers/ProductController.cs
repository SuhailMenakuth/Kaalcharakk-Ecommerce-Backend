﻿using CloudinaryDotNet;
using Kaalcharakk.Dtos.ProductDtos;
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

        [HttpGet("{id}")]
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

        [HttpGet("products")]
        [Authorize]
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

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery]ProductFilterDto filterCriteria)
        {
            try
            {
                var response = await _productService.GetProductsByFilterServiceAsync(filterCriteria);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}