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
        Task<BaseResponse> CreateProjectAsync(string adminId, CreateProjectRequest request);
        Task<BaseResponse> UpdateProjectAsync(string adminId, Guid projectId, UpdateProjectRequest request);
        Task<BaseResponse> DeleteProjectAsync(string adminId, Guid projectId);
        Task<List<ProjectResponse>> GetAllProjectAsync(string adminId);
        Task<ProjectResponse?> GetProjectByIdAsync(string adminId, Guid projectId);
        Task<BaseResponse> ChangeProjectStatusAsync(string adminId, ChangeProjectStatusRequest request);

    }
}
