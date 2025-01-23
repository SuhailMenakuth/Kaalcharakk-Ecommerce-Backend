using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Services.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateShippingAddress([FromBody] OrderAddressDto orderAddressDto)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var response = await _addressService.CreateShippingAddress(userId, orderAddressDto);

            if (response.StatusCode == 400 || response.StatusCode == 422)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("myadresses")]
        [Authorize]
        public async Task<IActionResult> GetShippingAddresses()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var response = await _addressService.GetShippingAddressesAsync(userId);

            if (response.StatusCode == 401)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpDelete("remove/{addressId}")]
        [Authorize]
        public async Task<IActionResult> RemoveShippingAddress(int addressId)
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);

            var isRemoved = await _addressService.RemoveShippingAddressByUserAsync(userId, addressId);

            if (!isRemoved)
                return BadRequest(new ApiResponse<string>(400, "Failed to remove the address or address not found"));

            return Ok(new ApiResponse<string>(200, "Address removed successfully"));
        }
    }
}
