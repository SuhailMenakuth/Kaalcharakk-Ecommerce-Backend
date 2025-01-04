using Kaalcharakk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
       






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                //.OnDelete(DeleteBehavior.Restrict);

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
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Role>()
                .HasData(
                new Role { RoleId = 1 , RoleName = "User"},
                new Role { RoleId = 2 , RoleName = "Admin"}
                );

            modelBuilder.Entity<Category>().HasData(
               new Category { CategoryId = 1, Name = "Male" },
               new Category { CategoryId = 2, Name = "Female" }
               );


            // category



        }


    }
}
