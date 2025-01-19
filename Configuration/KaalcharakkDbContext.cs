using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography.Xml;

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

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<ShippingAddress> shippingAddresses { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
                //.OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<User>(u => u.UserId)
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

            modelBuilder.Entity<Product>()
                .ToTable(tableBuilder =>
                {
                    tableBuilder.HasCheckConstraint("Product_Stock_NonNegative", "[Stock] >= 0");
                    tableBuilder.HasCheckConstraint("Product_Price_Nonnegative", "[Price] >= 0");
                });
            // category and product 
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Role>()
                .HasData(
                new Role { RoleId = 1, RoleName = "User" },
                new Role { RoleId = 2, RoleName = "Admin" }
                );

            modelBuilder.Entity<Category>().HasData(
               new Category { CategoryId = 1, Name = "Male" },
               new Category { CategoryId = 2, Name = "Female" }
               );


            //cart and user 
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            // cartItem and Cart 
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // cartItem and Product 
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // User and Wishlist (One-to-One)
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wishlist)
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Wishlist and WishlistItem (One-to-Many)
            modelBuilder.Entity<WishlistItem>()
                .HasOne(wi => wi.Wishlist)
                .WithMany(w => w.Items)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            // WishlistItem and Product (One-to-One)
            modelBuilder.Entity<WishlistItem>()
                .HasOne(wi => wi.Product)
                .WithMany()
                .HasForeignKey(wi => wi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShippingAddress>()
                 .HasOne(r => r.User)
                 .WithMany(r => r.ShippingAddresses)
                 .HasForeignKey(k => k.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(r => r.User)
                .WithMany(r => r.Orders)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(r => r.ShippingAddress)
                .WithMany()
                .HasForeignKey(k => k.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .Property(p => p.OrderStatus)
                .HasConversion(new EnumToStringConverter<OrderStatus>());

            modelBuilder.Entity<Order>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(12.2)");

            modelBuilder.Entity<OrderItem>()
                .HasOne(r => r.Order)
                .WithMany(r => r.OrderItems)
                .HasForeignKey(k => k.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(r => r.Product)
                .WithMany(r => r.OrderItems)
                .HasForeignKey(k => k.ProductId);

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(12,2)");



        }


    }
}
