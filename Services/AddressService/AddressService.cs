using AutoMapper;
using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Helpers.Response;
using Kaalcharakk.Models;
using Kaalcharakk.Repositories.AdressRepository;
using System;

namespace Kaalcharakk.Services.AddressService
{
    public class AddressService : IAddressService
    {

        private readonly IMapper _mapper;
        private readonly IAddressRepository _addressRepository;
        public AddressService( IMapper mapper , IAddressRepository  addressRepository)
        {
            _mapper = mapper;
            _addressRepository = addressRepository;
        }
        public async Task<ApiResponse<int>> CreateShippingAddress(int userId, OrderAddressDto orderAddressDTO)
        {
            try
            {
                if (orderAddressDTO == null)
                        
                {
                    return new ApiResponse<int>(400, "Bad Request,Address is null");


                }



                // A user can have atmost 3 address

                var addressCount = await _addressRepository.GetShippingAdressCount(userId);

                if (addressCount >= 3)
                {
                    return new ApiResponse<int>(422, "User cannot have more than 3 addresses.");
                }



                var newAddress = _mapper.Map<ShippingAddress>(orderAddressDTO);
                newAddress.UserId = userId;
                //await _context.ShippingAddresses.AddAsync(newAddress);
                //await _context.SaveChangesAsync();
                await _addressRepository.CreateAddressAsync(newAddress);
                return new ApiResponse<int>(200, "Successfully Created Shipping Address", newAddress.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<ViewAddressDto>>> GetShippingAddressesAsync(int userId)
        {
            


            var userAddress = await _addressRepository.GetUserWithShippingAddressesAsync(userId);

            if (userAddress == null)
            {
                return new ApiResponse<List<ViewAddressDto>>(401, "Invalid User");
            }

            
            var res = userAddress.ShippingAddresses
                                 .Select(address => _mapper.Map<ViewAddressDto>(address))
                                 .ToList();

            return new ApiResponse<List<ViewAddressDto>>(200, "Shipping Address Fetched Successfully", res);
        }

        public async Task<bool> RemoveShippingAddressByUserAsync(int userId, int addressId)
        {

            var user = await _addressRepository.GetUserWithShippingAddressesAsync(userId);

            
            if (user == null)
            {
                return false;
            }

            var address = user.ShippingAddresses.FirstOrDefault(x => x.Id == addressId);
            if (address == null)
            {
                return false;
            }
            return await _addressRepository.RemoveShippingAddressAsync(user, address);
        }




    }
}
