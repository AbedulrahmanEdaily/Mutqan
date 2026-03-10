using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.ProjectResponse;


namespace Mutqan.BLL.Services.Interface
{
    public interface IProjectMemberService:IScopedService
    {
        Task<BaseResponse> AddUserToProjectAsync(string requesterId, AddProjectMemberRequest request);
        Task<BaseResponse> RemoveUserFromProjectAsync(string requesterId, Guid projectId, string userId);
        Task<List<ProjectMemberResponse>> GetAllProjectMembersAsync(string requesterId, Guid projectId);
        Task<ProjectMemberResponse?> GetProjectMemberByIdAsync(string requesterId, Guid projectMemberId);
    }
}
