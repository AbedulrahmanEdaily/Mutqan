using Mutqan.DAL.DTO.Request.UserRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.UserResponse;


namespace Mutqan.BLL.Services.Interface
{
    public interface IManageUsersService : IScopedService
    {
        Task<List<UserRespnose>> GetUsersAsync();
        Task<UserDetailsResponse?> GetUserDetailsAsync(string userId);
        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    }
}
