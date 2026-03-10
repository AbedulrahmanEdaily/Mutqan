using Microsoft.AspNetCore.Identity;
using Mutqan.DAL.DTO.Request.TaskRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;

namespace Mutqan.BLL.Services.Interface
{
    public interface IProjectTaskService : IScopedService
    {
        Task<BaseResponse> CreateTaskAsync(string requesterId, CreateTaskRequest request);
        Task<BaseResponse> UpdateTaskAsync(string requesterId, Guid taskId, UpdateTaskRequest request);
        Task<BaseResponse> DeleteTaskAsync(string requesterId, Guid taskId);
        Task<BaseResponse> AddTaskToSprintAsync(string requesterId, AddTaskToSprintRequest request);
        Task<BaseResponse> RemoveTaskFromSprintAsync(string requesterId, Guid taskId);
        Task<BaseResponse> ChangeTaskPriorityAsync(string requesterId, Guid taskId, ChangeTaskPriorityRequest request);
        Task<BaseResponse> ChangeTaskStatusAsync(string userId, Guid taskId, ChangeTaskStatusRequest request);
        Task<BaseResponse> AssignTaskToDeveloperAsync(string requesterId, Guid taskId, AssignTaskToDeveloperRequest request);
        Task<PagintedResponse<ProjectTaskResponse>> GetAllTasksAsync(string requesterId, Guid projectId, int limit = 3, int page = 1);
        Task<TaskDetailsResponse?> GetTaskDetailsAsync(string requesterId, Guid taskId);
    }
}
