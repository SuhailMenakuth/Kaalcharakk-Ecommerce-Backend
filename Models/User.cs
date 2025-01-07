using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaalcharakk.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone must be 10 digits.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [ForeignKey("Role")]
        [AllowedValues(1,2)]
        public int RoleId { get; set; }
        public bool IsActived { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
       
        
        public Role Role { get; set; }
        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }





    }
}
