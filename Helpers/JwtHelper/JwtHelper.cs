using Kaalcharakk.Helpers.JwtHelper.JwtHelper;
using Kaalcharakk.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kaalcharakk.Helpers.JwtHelper.JwtHelper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            try
            {
                // Validate required configuration settings
                var secretKey = _configuration["Jwt:SecretKey"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var expiryInHours = int.TryParse(_configuration["Jwt:ExpiryInHours"], out var hours) ? hours : 2; // Default to 2 hours

                if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {

                    throw new Exception($"Missing JWT configuration values. SecretKey: {secretKey}, Issuer: {issuer}, Audience: {audience}");
                }

                // Generate the security key and credentials
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Define claims
                var claims = new[]
                {
               
            new Claim(ClaimTypes.Role, user.Role.RoleName),      // user role (role name)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID

            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // user ID (user identifier)
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName), // user's full name
            new Claim(ClaimTypes.Email, user.Email), // user's email (email claim)
                        };

                // Create the token
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(expiryInHours),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception (you can implement a logger here)
                throw new Exception("An error occurred while generating the token.", ex);
            }
        }

        // pending
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false // Allow expired tokens
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }


}
