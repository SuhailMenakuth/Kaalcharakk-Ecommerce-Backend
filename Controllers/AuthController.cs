using Azure.Core;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Kaalcharakk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {

                if(registerDto == null)
                {
                    return BadRequest(new {message = "form are incomplete" });
                }

                var result = await _authService.RegisterAsync(registerDto);
                if (result)
                {
                    return Ok(new { message = "Registration Successful" });
                }
                else
                {
                    return BadRequest(new { message = "Registraion failed" });
                }
                
            }
            catch (Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }





        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logindto)
        {
            try
            {
                if (logindto == null || string.IsNullOrEmpty(logindto.Username) || string.IsNullOrEmpty(logindto.Password))
                {
                    return BadRequest(new { message = "Username and Password are required" });
                }

                var (accessToken, refreshToken) = await _authService.LoginAsync(logindto);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

               


                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.None, 
                    Expires = DateTime.UtcNow.AddMinutes(15) 
                };

                Response.Cookies.Append("accessToken", accessToken, cookieOptions);

                cookieOptions.Expires = DateTime.UtcNow.AddMonths(1); 
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);


                return Ok(new { message = "Login Successful", accessToken, refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("refresh/token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { message = "Refresh token is missing" });
                }

                var newAccessToken = await _authService.RefreshTokenAsync(refreshToken);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.None, 
                    Expires = DateTime.UtcNow.AddMinutes(15) 
                };

                Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);

                return Ok(new { message = "Token refreshed successfully", accessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}
