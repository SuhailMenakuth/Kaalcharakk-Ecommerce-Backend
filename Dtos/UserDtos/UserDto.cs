using Kaalcharakk.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Dtos.UserDtos
{
    public class UserDto
    {
        public int UserId { get; set; }

      
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string Email { get; set; }

        
        public string Phone { get; set; }

        
        public string PasswordHash { get; set; }

       
        public int RoleId { get; set; }
        public bool IsActived { get; set; } = true;
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 


        public Role Role { get; set; }
        public Cart Cart { get; set; }

        public ICollection<CartItem> cartItems { get; set; }

    }
}
