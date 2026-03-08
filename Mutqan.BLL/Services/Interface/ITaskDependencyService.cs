using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface ITaskDependencyService : IScopedService
    {
        Task<BaseResponse> AddDependencyAsync(string adminId, Guid taskId, Guid dependsOnTaskId);
        Task<BaseResponse> RemoveDependencyAsync(string adminId, Guid taskId, Guid dependsOnTaskId);
        Task<List<TaskDependencyResponse>> GetDependenciesAsync(string adminId, Guid taskId);
    }
}
