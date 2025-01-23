using Kaalcharakk.Dtos.UserDtos;
using Kaalcharakk.Helpers.Response;

namespace Kaalcharakk.Services.UserService
{
    public interface IUserService
    {
        Task <ApiResponse<string>> UpdateUserStatusServiceAsync(int userId , bool block);

        Task <ApiResponse<UserViewDto>> FetchUserServiceAsync(int userId );

        Task <ApiResponse<List<UserViewDto>>> FetchAllUserServiceAsync();

        Task<ApiResponse<MyDetailsDto>> FetchMyDetailsAsync(int userId);

        
        
    }
}
