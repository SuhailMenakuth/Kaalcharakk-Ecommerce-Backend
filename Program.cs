using Kaalcharakk.Configuration;
using Kaalcharakk.Helpers.CloudinaryHelper;
using Kaalcharakk.Helpers.JwtHelper.JwtHelper;
using Kaalcharakk.Mapper;
using Kaalcharakk.Middleware;
using Kaalcharakk.Repositories.AuthRepository;
using Kaalcharakk.Repositories.ProductRepository;
using Kaalcharakk.Services.Authentication;
using Kaalcharakk.Services.ProductService;



//using Kaalcharakk.Repositories.AuthRepository;
//using Kaalcharakk.Services.Authentication;
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

            // Add services to the container.
            builder.Services.AddDbContext<KaalcharakkDbContext>(options =>
                  options.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.EnableRetryOnFailure())
            );
            builder.Services.AddAutoMapper(typeof(MapperProfile));
            //builder.Services.AddLogging();

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddLogging();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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









            // User Secrets configuration (only for local development)
            builder.Configuration.AddUserSecrets<Program>();

            // JWT Configuration      
            //var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);
            var audience = builder.Configuration["Jwt:Audience"];
            var issuer = builder.Configuration["Jwt:Issuer"];


            // Configure JWT Authentication
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
                    ClockSkew = TimeSpan.Zero // Optional: Removes the default 5-minute clock skew
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Add authentication before authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdentificationMiddleWare>();
            app.MapControllers();

            app.Run();
        }
    }
}
