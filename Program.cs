using Kaalcharakk.Configuration;
using Kaalcharakk.Helpers.CloudinaryHelper;
using Kaalcharakk.Helpers.JwtHelper.JwtHelper;
using Kaalcharakk.Helpers.RazorPayHelper;
using Kaalcharakk.Mapper;
using Kaalcharakk.Middleware;
using Kaalcharakk.Repositories.AdressRepository;
using Kaalcharakk.Repositories.AuthRepository;
using Kaalcharakk.Repositories.CartRepository;
using Kaalcharakk.Repositories.OrderRepository;
using Kaalcharakk.Repositories.ProductRepository;
using Kaalcharakk.Repositories.UserRepository;
using Kaalcharakk.Repositories.WishlistRepository;
using Kaalcharakk.Services.AddressService;
using Kaalcharakk.Services.Authentication;
using Kaalcharakk.Services.CartService;
using Kaalcharakk.Services.OrderService;
using Kaalcharakk.Services.ProductService;
using Kaalcharakk.Services.UserService;
using Kaalcharakk.Services.WishlistServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Kaalcharakk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<KaalcharakkDbContext>(options =>
                  options.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.EnableRetryOnFailure())
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .AllowAnyHeader(); 

                    });
            });




            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<IRazorpayHelper, RazorpayHelper>();

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUserRepositoy ,UserRepository>();




            builder.Services.AddLogging();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Kaalcharakk", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });



            builder.Configuration.AddUserSecrets<Program>();
            var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);
            var audience = builder.Configuration["Jwt:Audience"];
            var issuer = builder.Configuration["Jwt:Issuer"];
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigin");
            app.UseMiddleware<TokenCookieMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdentificationMiddleWare>();
            app.MapControllers();

            app.Run();
        }
    }
}
