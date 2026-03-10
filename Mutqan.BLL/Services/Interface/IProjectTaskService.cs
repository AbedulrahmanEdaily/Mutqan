using Microsoft.AspNetCore.Identity;
using Mutqan.DAL.DTO.Request.TaskRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        Task<List<ProjectTaskResponse>> GetAllTasksAsync(string requesterId, Guid projectId);
        Task<TaskDetailsResponse?> GetTaskDetailsAsync(string requesterId, Guid taskId);
    }
}
