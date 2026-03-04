using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.ProjectResponse;


namespace Mutqan.BLL.Services.Interface
{
    public interface IProjectMemberService:IScopedService
    {
        Task<BaseResponse> AddUserToProjectAsync(string adminId, AddProjectMemberRequest request);
        Task<BaseResponse> RemoveUserFromProjectAsync(string adminId, Guid projectId, string userId);
        Task<List<ProjectMemberResponse>> GetAllProjectMembersAsync(string adminId, Guid projectId);
        Task<ProjectMemberResponse?> GetProjectMemberByIdAsync(string adminId, Guid projectMemberId);
    }
}
