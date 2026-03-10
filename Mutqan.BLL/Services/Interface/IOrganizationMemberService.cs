using Microsoft.AspNetCore.Mvc;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.OrganizationResponse;

namespace Mutqan.BLL.Services.Interface
{
    public interface IOrganizationMemberService : IScopedService
    {
        Task<BaseResponse> AddUserToOrganizationAsync(string requesterId,OrganizationMemberRequest request);
        Task<BaseResponse> RemoveUserFromOrganizationAsync(string requesterId,string userId);
        Task<OrganizationMemberResponse?> GetMemberByUserIdAsync(string userId,string userRequested);
        Task<PagintedResponse<OrganizationMemberResponse>> GetAllMemberAsync(string requesterId, int limit = 3, int page = 1);
    }
}
