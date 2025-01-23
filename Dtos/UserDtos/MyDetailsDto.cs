using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.OrderDtos;

namespace Kaalcharakk.Dtos.UserDtos
{
    public class MyDetailsDto
    {

   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<ViewAddressDto>? ViewUserAddress { get; set; }

       
    }
}
