using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TaskResponse;
using Mutqan.DAL.Repository.Interface;
namespace Mutqan.BLL.Services.Class
{
    public class TaskDependencyService : ITaskDependencyService
    {
        private readonly ITaskDependencyRepository _taskDependencyRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public TaskDependencyService(
             ITaskDependencyRepository taskDependencyRepository
             ,IProjectTaskRepository projectTaskRepository
            ,IProjectMemberRepository projectMemberRepository
            )
        {
            _taskDependencyRepository = taskDependencyRepository;
            _projectTaskRepository = projectTaskRepository;
            _projectMemberRepository = projectMemberRepository;
        }
        public async Task<BaseResponse> AddDependencyAsync(string requesterId, Guid taskId, Guid dependsOnTaskId)
        {
            if (taskId == dependsOnTaskId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task cannot depend on itself"
                };
            }
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var dependsOnTask = await _projectTaskRepository.GetTaskAsync(dependsOnTaskId);
            if (dependsOnTask is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Dependency task not found"
                };
            }
            if (task.ProjectId != dependsOnTask.ProjectId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Tasks must belong to the same project"
                };
            }
            if (dependsOnTask.Status == DAL.Models.TaskStatus.Done)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Dependency task is already completed"
                };
            }
            var exists = await _taskDependencyRepository.IsDependencyExistsAsync(taskId, dependsOnTaskId);
            if (exists)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Dependency already exists"
                };
            }
            var hasCircular = await _taskDependencyRepository.HasCircularDependencyAsync(taskId, dependsOnTaskId);
            if (hasCircular)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "Circular dependency detected" 
                };
            }
            await _taskDependencyRepository.CreateAsync(taskId, dependsOnTaskId);
            return new BaseResponse
            {
                Success = true,
                Message = "Task dependency added successfully"
            };
        }
        public async Task<BaseResponse> RemoveDependencyAsync(string requesterId, Guid taskId, Guid dependsOnTaskId)
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
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var dependsOnTask = await _projectTaskRepository.GetTaskAsync(dependsOnTaskId);
            if (dependsOnTask is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Dependency task not found"
                };
            }
            var result = await _taskDependencyRepository.RemoveAsync(taskId, dependsOnTaskId);
            if (!result)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Dependency not found"
                };
            }
            return new BaseResponse
            {
                Success = true,
                Message = "Dependency task deleted successfully"
            };
        }
        public async Task<List<TaskDependencyResponse>> GetDependenciesAsync(string requesterId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return [];
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, requesterId);
            if (!isProjectMember)
            {
                return [];
            }
            var dependencies = await _taskDependencyRepository.GetAllAsync(taskId);
            return dependencies.Adapt<List<TaskDependencyResponse>>();
        }
    }
}
