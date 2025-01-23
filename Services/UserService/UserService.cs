using AutoMapper;
using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Dtos.UserDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.AdressRepository;
using Kaalcharakk.Repositories.OrderRepository;
using Kaalcharakk.Repositories.UserRepository;
using Kaalcharakk.Services.AddressService;
using Kaalcharakk.Services.OrderService;

namespace Kaalcharakk.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepositoy _userRepository;
        private readonly IAddressService _addressService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public UserService(IUserRepositoy userRepository , IOrderService orderService ,IAddressService addressService , IMapper mapper)
        {
            _userRepository = userRepository;
            _addressService = addressService;
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> UpdateUserStatusServiceAsync(int userId, bool block)
        {
            try
            {
                var user = await _userRepository.FetchUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<string>(404, "error", "User not found");
                }

                if (block)
                {
                    user.IsActived = false; 
                    await _userRepository.UpdateUserAsync(user);

                    
                    return new ApiResponse<string>(200, "success", $"User blocked successfully");
                } 
               else {
                    user.IsActived = true; 
                    await _userRepository.UpdateUserAsync(user);

                    
                    return new ApiResponse<string>(200, "success", $"User Unblocked successfully");
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in UpdateUserStatusServiceAsync: {ex.Message}");

                
                return new ApiResponse<string>(500, "error", "An error occurred while updating user status");
            }
        }


        public async Task<ApiResponse<UserViewDto>> FetchUserServiceAsync(int userId)
        {
            // Fetch the user details
            var user = await _userRepository.FetchUserByIdAsync(userId); // Ensure this method exists in your repository
            if (user == null)
            {
                return new ApiResponse<UserViewDto>(404, "User not found");
            }

            // Fetch the user's shipping addresses
            var userAddressResponse = await _addressService.GetShippingAddressesAsync(userId);
            //if (userAddressResponse.StatusCode != 200)
            //{
            //    return new ApiResponse<UserViewDto>(401, "Error fetching user addresses");
            //}

            // Fetch the user's orders
            var userOrders = await _orderService.GetOrdersAsync(userId);

            // Construct the UserViewDto
            var userViewDto = new UserViewDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                IsActived = user.IsActived,
                ViewUserAddress = userAddressResponse.Data,
                orders = userOrders
            };

            return new ApiResponse<UserViewDto>(200, "User data fetched successfully", userViewDto);
        }

        public async Task<ApiResponse<List<UserViewDto>>> FetchAllUserServiceAsync()
        {
            try
            {
                // Fetch all users from the repository
                var users = await _userRepository.FetchAllUserAsync();

                // Check if users are found
                if (users == null || !users.Any())
                {
                    return new ApiResponse<List<UserViewDto>>(404, "error", error: "there is no users");
                    
                }

                // Map users to UserViewDto
                var userDtos = _mapper.Map<List<UserViewDto>>(users);

                // Return success response
                return new ApiResponse<List<UserViewDto>>(200, "sucsses", userDtos);
              
            }
            catch (Exception ex)
            {
                // Handle exceptions and return failure response
                return new ApiResponse<List<UserViewDto>>(500,"internal server error");
               
            }
        }

        public async Task<ApiResponse<MyDetailsDto>> FetchMyDetailsAsync(int userId)
        {
            var myDetails = _userRepository.FetchUserByIdAsync(userId);

            var myDetailsDto = _mapper.Map<MyDetailsDto>(myDetails);
            if(myDetails == null)
            {
                return new ApiResponse<MyDetailsDto>(404, "not found", error: "internal server error");

            }
            return new ApiResponse<MyDetailsDto>(200, "success", myDetailsDto);
        }


    }
}
