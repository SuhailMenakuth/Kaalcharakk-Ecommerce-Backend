using AutoMapper;
using BCrypt.Net;
using Kaalcharakk.Configuration;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Helpers.JwtHelper.JwtHelper;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.AuthRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;

namespace Kaalcharakk.Services.Authentication
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        //private readonly ILogger<AuthService> _logger;


        public AuthService(IAuthRepository authRepository , IJwtHelper jwtHelper ,IMapper mapper)
        {
             
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
            //_logger = logger;

        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
               
                //if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                if (await _authRepository.GetUserByEmailAsync(registerDto.Email) != null)
                {
                    throw new Exception("Email already in use");
                }
                if (await _authRepository.GetUserByPhoneAsync(registerDto.Phone) == true)
                {
                    throw new Exception("Phone number already in use");
                } 

                var PasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,15}$");

                if (!PasswordRegex.IsMatch(registerDto.PasswordHash))
                {
                    throw new Exception("Password doesn't meet the required criteria");
                }



                registerDto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.PasswordHash);
               
                var user = _mapper.Map<User>(registerDto);
                user.CreatedAt = DateTime.UtcNow; 
                user.RoleId = 1;
                user.IsActived = true;


                Console.WriteLine($"Hashed Password: {user.PasswordHash}");

                await _authRepository.AddUserAsync(user);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"database error:{ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }
        // original
        //public async Task<string> LoginAsync(LoginDto loginDto)
        //{
        //    try
        //    {

        //        var user = await _authRepository.GetUserByEmailAsync(loginDto.Username);
        //        if ( user != null && user.IsActived == false)
        //        {
        //            throw new Exception("you are blocked");
        //        }


        //        if (user == null || BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash) == false)
        //        {
        //            throw new Exception("username or password is wrong");
        //        }

        //        return _jwtHelper.GenerateToken(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"database error:{ex.InnerException?.Message ?? ex.Message}");
        //        throw;
        //    }
        //}



        // second 
        //public async Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto loginDto)
        //{
        //    var user = await _authRepository.GetUserByEmailAsync(loginDto.Username);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        //    {
        //        throw new Exception("Invalid username or password");
        //    }

        //    if (!user.IsActived)
        //    {
        //        throw new Exception("You are blocked");
        //    }

        //    var accessToken = _jwtHelper.GenerateToken(user);
        //    var refreshToken = _jwtHelper.GenerateRefreshToken();

        //    // Save the Refresh Token in the database
        //    await _authRepository.SaveRefreshTokenAsync(user.UserId, refreshToken, DateTime.UtcNow.AddDays(7));

        //    return (accessToken, refreshToken);
        //}

        //public async Task<string> RefreshTokenAsync(string refreshToken)
        //{
        //    var storedToken = await _authRepository.GetRefreshTokenAsync(refreshToken);
        //    if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
        //    {
        //        throw new Exception("Invalid or expired refresh token");
        //    }

        //    var user = await _authRepository.GetUserByIdAsync(storedToken.UserId);
        //    if (user == null)
        //    {
        //        throw new Exception("User not found");
        //    }

        //    // Generate a new Access Token
        //    return _jwtHelper.GenerateToken(user);
        //}




        public async Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto loginDto)
        {
            // Generate tokens
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            if (!user.IsActived)
            {
                throw new Exception("You are blocked");
            }

            var accessToken = _jwtHelper.GenerateToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            // Save the refresh token in the database
            await _authRepository.SaveRefreshTokenAsync(user.UserId, refreshToken, DateTime.UtcNow.AddMonths(1));

            return (accessToken, refreshToken);
        }


        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            // Retrieve the refresh token from the database
            var storedToken = await _authRepository.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token.");
            }

            // Retrieve user details associated with the refresh token

            // check if this correct can we check user id from the refresh token will share an example refresh token 
            var user = await _authRepository.GetUserByIdAsync(storedToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Generate new access token
            string newAccessToken = _jwtHelper.GenerateToken(user);

            return newAccessToken;  // Returning new access token
        }


    }
}