using Kaalcharakk.Models;
using Kaalcharakk.Models.Category;
using Kaalcharakk.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Kaalcharakk.Configuration
{
    public class KaalcharakkDbContext : DbContext
    {
        public KaalcharakkDbContext(DbContextOptions<KaalcharakkDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(u => u.RoleId)
               .HasDefaultValue(2);

                entity.Property(u => u.IsActived)
                .HasDefaultValue(true);


                entity.Property(u => u.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
                    
                entity.HasIndex(u => u.Email)
                .IsUnique();

                entity.HasIndex(u => u.Phone)
                .IsUnique();
            });

            modelBuilder.Entity<Role>().HasData(

                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Customer" }

                );


            // category

            modelBuilder.Entity<ParentCategory>()
                .HasMany(pc => pc.SubCategories)
                .WithOne(sc => sc.ParentCategory)
                .HasForeignKey(sc => sc.ParentCategoryId);


            modelBuilder.Entity<SubCategory>()
                .HasMany<Product>()
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductSizes)
                .WithOne(ps => ps.Product)
                .HasForeignKey(ps => ps.ProductId);


            modelBuilder.Entity<ParentCategory>().HasData(
                new ParentCategory
                {
                    ParentCategoryId = 1,
                    CategoryName = "Men",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDelete = false
                },
                new ParentCategory
                {
                    ParentCategoryId = 2,
                    CategoryName = "Women",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDelete = false
                }
                );


            modelBuilder.Entity<SubCategory>().HasData(
        new SubCategory
        {
            SubCategoryId = 1,
            CategoryName = "Scandals",
            ParentCategoryId = 1, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        },
        new SubCategory
        {
            SubCategoryId = 2,
            CategoryName = "Shoes",
            ParentCategoryId = 1, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        },
        new SubCategory
        {
            SubCategoryId = 3,
            CategoryName = "Scandals",
            ParentCategoryId = 2, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        },
        new SubCategory
        {
            SubCategoryId = 4,
            CategoryName = "Shoes",
            ParentCategoryId = 2, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        }
    );




        }


    }
}
