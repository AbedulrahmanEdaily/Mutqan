using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.ProjectResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface IProjectService:IScopedService
    {
        Task<BaseResponse> CreateProjectAsync(string requesterId, CreateProjectRequest request);
        Task<BaseResponse> UpdateProjectAsync(string requesterId, Guid projectId, UpdateProjectRequest request);
        Task<BaseResponse> DeleteProjectAsync(string requesterId, Guid projectId);
        Task<List<ProjectResponse>> GetAllProjectAsync(string requesterId);
        Task<ProjectResponse?> GetProjectByIdAsync(string requesterId, Guid projectId);
        Task<BaseResponse> ChangeProjectStatusAsync(string requesterId, ChangeProjectStatusRequest request);

    }
}
