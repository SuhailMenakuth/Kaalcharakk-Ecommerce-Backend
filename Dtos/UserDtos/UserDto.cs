using Kaalcharakk.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Dtos.AdressDtos;

namespace Kaalcharakk.Dtos.UserDtos
{
    public class UserViewDto
    {
        public int UserId { get; set; }

      
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string Email { get; set; }

        
        public string Phone { get; set; }
       
        public int RoleId { get; set; }
        public bool IsActived { get; set; } = true;
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 

        
        public Role Role { get; set; }

        public List<ViewAddressDto>? ViewUserAddress { get; set; }

        public List<OrderViewDto>? orders { get; set; } 

    }
}
