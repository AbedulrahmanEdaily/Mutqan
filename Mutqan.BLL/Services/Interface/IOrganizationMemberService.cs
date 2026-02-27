using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using Mutqan.DAL.Models;

namespace Mutqan.BLL.Services.Interface
{
    public interface IOrganizationMemberService : IScopedService
    {
        Task<BaseResponse> AddUserToOrganizationAsync(string adminId,OrganizationMemberRequest request);
        Task<BaseResponse> RemoveUserFromOrganizationAsync(string adminId,string userId);
        Task<OrganizationMemberResponse?> GetMemberByUserIdAsync(string userId,string userRequested);
        Task<List<OrganizationMemberResponse>> GetAllMemberAsync(string requesterId);
    }
}
