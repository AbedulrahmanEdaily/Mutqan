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
        Task<BaseResponse> CreateTaskAsync(string adminId, CreateTaskRequest request);
        Task<BaseResponse> UpdateTaskAsync(string adminId, Guid taskId, UpdateTaskRequest request);
        Task<BaseResponse> DeleteTaskAsync(string adminId, Guid taskId);
        Task<BaseResponse> AddTaskToSprintAsync(string adminId, AddTaskToSprintRequest request);
        Task<BaseResponse> RemoveTaskFromSprintAsync(string adminId, Guid taskId);
        Task<BaseResponse> ChangeTaskPriorityAsync(string adminId, Guid taskId, ChangeTaskPriorityRequest request);
        Task<BaseResponse> ChangeTaskStatusAsync(string userId, Guid taskId, ChangeTaskStatusRequest request);
        Task<BaseResponse> AssignTaskToDeveloperAsync(string adminId, Guid taskId, AssignTaskToDeveloperRequest request);
        Task<List<ProjectTaskResponse>> GetAllTasksAsync(string adminId, Guid projectId);
        Task<TaskDetailsResponse?> GetTaskDetailsAsync(string adminId, Guid taskId);
    }
}
