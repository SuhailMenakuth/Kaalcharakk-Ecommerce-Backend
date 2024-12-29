using Kaalcharakk.Models;
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
                


            
        }


    }
}
