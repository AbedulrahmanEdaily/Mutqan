using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface ITaskDependencyService : IScopedService
    {
        Task<BaseResponse> AddDependencyAsync(string requesterId, Guid taskId, Guid dependsOnTaskId);
        Task<BaseResponse> RemoveDependencyAsync(string requesterId, Guid taskId, Guid dependsOnTaskId);
        Task<List<TaskDependencyResponse>> GetDependenciesAsync(string requesterId, Guid taskId);
    }
}
