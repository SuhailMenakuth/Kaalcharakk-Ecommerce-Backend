using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("block/user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            var response = await _userService.UpdateUserStatusServiceAsync(userId, block: true);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            else if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }

        }

        [HttpPatch("unblock/user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnBlockUser(int userId)
        {
           var  response = await _userService.UpdateUserStatusServiceAsync(userId, block: false);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            else if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }

        }

        [HttpGet("user/details")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FetchlUserDetailsById(int userId)
        {
            var response = await _userService.FetchUserServiceAsync(userId);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }



        [HttpGet("my/details")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> FetchMyDetails()
        {
            var userId = Convert.ToInt32(HttpContext.Items["UserId"]);
            var response = await _userService.FetchMyDetailsAsync(userId);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }




        [HttpGet("allusers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FetchAllUsers()
        {
            var response = await _userService.FetchAllUserServiceAsync();
            return Ok(response);    
        }
        


    }
}
