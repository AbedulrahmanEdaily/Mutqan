using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.TaskRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Class;
using Mutqan.DAL.Repository.Interface;


namespace Mutqan.BLL.Services.Class
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskDependencyRepository _taskDependencyRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public ProjectTaskService(
             IProjectTaskRepository projectTaskRepository
            ,IProjectRepository projectRepository
            ,IProjectMemberRepository projectMemberRepository
            ,ISprintRepository sprintRepository
            ,UserManager<ApplicationUser> userManager
            ,ITaskDependencyRepository taskDependencyRepository
            ,IOrganizationMemberRepository organizationMemberRepository
            )
        {
            _projectTaskRepository = projectTaskRepository;
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _sprintRepository = sprintRepository;
            _userManager = userManager;
            _taskDependencyRepository = taskDependencyRepository;
            _organizationMemberRepository = organizationMemberRepository;
        }
        public async Task<BaseResponse> CreateTaskAsync(string adminId, CreateTaskRequest request)
        {
            var project = await _projectRepository.FindByIdAsync(request.ProjectId);
            if(project is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(project.Id, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (request.EstimatedEndDate.HasValue &&request.EstimatedEndDate <= request.EstimatedStartDate)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "DueDate must be after StartDate" 
                };
            }
            var result = request.Adapt<ProjectTask>();
            await _projectTaskRepository.CreateAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Task created successfully"
            };
        }
        public async Task<BaseResponse> UpdateTaskAsync(string adminId, Guid taskId, UpdateTaskRequest request)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "task not foudn"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (request.EstimatedDueDate.HasValue && request.EstimatedStartDate.HasValue && request.EstimatedDueDate <= request.EstimatedStartDate)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Due date must be after start date"
                };
            }
            if(task.Status != DAL.Models.TaskStatus.Backlog)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task can only be edited while in backlog"
                };
            }
            request.Adapt(task);
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task updated successfully"
            };
        }
        public async Task<BaseResponse> DeleteTaskAsync(string adminId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if(task.Status != DAL.Models.TaskStatus.Backlog)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task can only be deleted while in backlog"
                };
            }
            await _projectTaskRepository.DeleteAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task deleted successfully"
            };
        }
        public async Task<BaseResponse> AddTaskToSprintAsync(string adminId, AddTaskToSprintRequest request)
        {
            var sprint = await _sprintRepository.FindByIdAsync(request.SprintId);
            if (sprint is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(sprint.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var task = await _projectTaskRepository.GetTaskAsync(request.TaskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            if (task.Status != DAL.Models.TaskStatus.Backlog)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task is not in backlog"
                };
            }
            if (task.ProjectId != sprint.ProjectId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task and sprint do not belong to the same project"
                };
            }
            if (sprint.Status != SprintStatus.Active)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint is not active"
                };
            }

            if (task.SprintId.HasValue)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task is already assigned to a sprint"
                };
            }
            var hasUncompletedDependencies = await _taskDependencyRepository.HasUncompletedDependenciesAsync(task.Id);
            if (hasUncompletedDependencies)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task has uncompleted dependencies"
                };
            }
            task.SprintId = sprint.Id;
            task.Status = DAL.Models.TaskStatus.Todo;
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task assigned to sprint successfully"
            };
        }
        public async Task<BaseResponse> RemoveTaskFromSprintAsync(string adminId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (task.SprintId is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task is not in a sprint"
                };
            }
            if (task.Status == DAL.Models.TaskStatus.Done)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Completed task cannot be removed from sprint"
                };
            }
            task.SprintId = null;
            task.Status = DAL.Models.TaskStatus.Backlog;
            task.ActualStartDate = null;
            task.ActualEndDate = null;
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task removed from sprint successfully"
            };
        }
        public async Task<BaseResponse> ChangeTaskPriorityAsync(string adminId, Guid taskId, ChangeTaskPriorityRequest request)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            task.Priority = request.Priority;
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse { 
                Success = true, 
                Message = "Priority changed successfully" 
            };
        }
        public async Task<BaseResponse> ChangeTaskStatusAsync(string userId, Guid taskId, ChangeTaskStatusRequest request)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, userId);
            var isAssignedDeveloper = task.AssignedToUserId == userId;
            if (!isProjectManager && !isAssignedDeveloper)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (request.Status == DAL.Models.TaskStatus.Backlog)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "Use remove-sprint endpoint" 
                };
            }
            if (isAssignedDeveloper && !isProjectManager && request.Status == DAL.Models.TaskStatus.Done)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "Developer can't set task as done" 
                };
            }
            if (task.Status == DAL.Models.TaskStatus.Backlog)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task must be added to a sprint first"
                };
            }
            if (task.Status == request.Status)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task is already in this status"
                };
            }
            if (request.Status == DAL.Models.TaskStatus.InProgress)
                task.ActualStartDate = DateTime.UtcNow;
            if (request.Status == DAL.Models.TaskStatus.Done)
                task.ActualEndDate = DateTime.UtcNow;
            task.Status = request.Status;
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Status changed successfully"
            };
        }
        public async Task<BaseResponse> AssignTaskToDeveloperAsync(string adminId, Guid taskId, AssignTaskToDeveloperRequest request)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, adminId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (request.DeveloperId is null)
            {
                if (task.AssignedToUserId is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Task is not assigned to anyone"
                    };
                }
                task.AssignedToUserId = null;
                await _projectTaskRepository.UpdateAsync(task);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Developer unassigned successfully"
                };
            }
            if (task.Status == DAL.Models.TaskStatus.InProgress ||
                task.Status == DAL.Models.TaskStatus.Review ||
                task.Status == DAL.Models.TaskStatus.Done)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cannot change assignee while task is in progress or completed"
                };
            }
            var user = await _userManager.FindByIdAsync(request.DeveloperId);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Developer not found"
                };
            }
            var isDeveloper = await _projectMemberRepository.IsDeveloperAsync(task.ProjectId, request.DeveloperId);
            if (!isDeveloper)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User is not a developer in this project"
                };
            }
            task.AssignedToUserId = request.DeveloperId;
            await _projectTaskRepository.UpdateAsync(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task assigned to developer successfully"
            };
        }
        public async Task<List<ProjectTaskResponse>> GetAllTasksAsync(string adminId, Guid projectId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if (project is null)
            {
                return [];
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(projectId, adminId);
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            if (!isProjectMember && !isOrganizationAdmin)
            {
                return [];
            }
            var tasks = await _projectTaskRepository.GetAllAsync(projectId);
            return tasks.Adapt<List<ProjectTaskResponse>>();
        }
        public async Task<TaskDetailsResponse?> GetTaskDetailsAsync(string adminId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskDetailsAsync(taskId);
            if(task is null)
            {
                return null;
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, adminId);
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, task.Project.OrganizationId);
            if (!isProjectMember && !isOrganizationAdmin)
            {
                return null;
            }
            return task.Adapt<TaskDetailsResponse>();
        }
    }
}