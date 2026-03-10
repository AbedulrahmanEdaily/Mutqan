using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.SprintRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.SprintResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
namespace Mutqan.BLL.Services.Class
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly INotificationService _notificationService;

        public SprintService(
             ISprintRepository sprintRepository
            ,IProjectRepository projectRepository
            ,IProjectMemberRepository projectMemberRepository
            ,IProjectTaskRepository projectTaskRepository
            ,IOrganizationMemberRepository organizationMemberRepository
            ,INotificationService notificationService
            )
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectTaskRepository = projectTaskRepository;
            _organizationMemberRepository = organizationMemberRepository;
            _notificationService = notificationService;
        }
        public async Task<List<SprintResponse>> GetAllSprintsAsync(string requesterId, Guid projectId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if (project is null)
            {
                return new List<SprintResponse>();
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(projectId,requesterId);
            var IsOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(requesterId, project.OrganizationId);
            if (!isProjectMember && !IsOrganizationAdmin)
            {
                return new List<SprintResponse>();
            }
            var sprints = await _sprintRepository.GetAllAsync(projectId);
            return sprints.Adapt<List<SprintResponse>>();
        }
        public async Task<SprintDetailsResponse?> GetSprintDetailsAsync(string requesterId, Guid sprintId)
        {
            var sprint = await _sprintRepository.FindByIdAsync(sprintId);
            if (sprint is null)
            {
                return null;
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(sprint.ProjectId, requesterId);
            var IsOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(requesterId, sprint.Project.OrganizationId);
            if (!isProjectMember && !IsOrganizationAdmin)
            {
                return null;
            }
            return sprint.Adapt<SprintDetailsResponse>();
        }
        public async Task<BaseResponse> CreateSprintAsync(string requesterId,CreateSprintRequest request)
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
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(project.Id, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (request.EndDate <= request.StartDate)
            {
                return new BaseResponse { 
                    Success = false, 
                    Message = "EndDate must be after StartDate" 
                };
            }
            var result = request.Adapt<Sprint>();
            await _sprintRepository.CreateAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Sprint created successfully"
            };
        }
        public async Task<BaseResponse> UpdateSprintAsync(string requesterId,Guid sprintId,UpdateSprintRequest request)
        {
            var sprint = await _sprintRepository.FindByIdAsync(sprintId);
            if (sprint is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(sprint.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (sprint.Status == SprintStatus.Completed)
            {
                return new BaseResponse { 
                    Success = false, 
                    Message = "Can't update a completed sprint" 
                };
            }
            if (request.EndDate.HasValue && request.StartDate.HasValue
            && request.EndDate <= request.StartDate)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "EndDate must be after StartDate"
                };
            }
            request.Adapt(sprint);
            await _sprintRepository.UpdateAsync(sprint);
            return new BaseResponse
            {
                Success = true,
                Message = "Sprint updated successfully"
            };
        }
        public async Task<BaseResponse> DeleteSprintAsync (string requesterId, Guid sprintId)
        {
            var sprint = await _sprintRepository.FindByIdAsync(sprintId);
            if (sprint is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(sprint.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (sprint.Status == SprintStatus.Active)
            {
                return new BaseResponse { 
                    Success = false, 
                    Message = "Can't delete an active sprint" 
                };
            }
            await _projectTaskRepository.MoveUncompletedTasksToBacklogAsync(sprintId);
            await _sprintRepository.DeleteAsync(sprint);
            return new BaseResponse
            {
                Success = true,
                Message = "Sprint deleted successfully"
            };
        }
        public async Task<BaseResponse> StartSprintAsync (string requesterId, Guid sprintId)
        {
            var sprint = await _sprintRepository.FindByIdAsync(sprintId);
            if (sprint is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(sprint.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (await _sprintRepository.HasProjectActiveSprintAsync(sprint.ProjectId))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project already has an active sprint"
                };
            }
            sprint.ActualStartDate = DateTime.UtcNow;
            sprint.Status = SprintStatus.Active;
            await _sprintRepository.UpdateAsync(sprint);
            var projectMembers = await _projectMemberRepository.GetAllAsync(sprint.ProjectId);
            foreach (var member in projectMembers)
            {
                await _notificationService.SendNotificationAsync(
                    member.UserId,
                    $"Sprint '{sprint.Name}' has started",
                    NotificationType.SprintStarted,
                    null
                );
            }
            return new BaseResponse
            {
                Success = true,
                Message = "Sprint started successfully"
            };
        }
        public async Task<BaseResponse> CompleteSprintAsync(string requesterId, Guid sprintId)
        {
            var sprint = await _sprintRepository.FindByIdAsync(sprintId);
            if (sprint is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(sprint.ProjectId, requesterId);
            if (!isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if(sprint.Status != SprintStatus.Active)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Sprint is not active"
                };
            }
            await _projectTaskRepository.MoveUncompletedTasksToBacklogAsync(sprintId);
            sprint.ActualEndDate = DateTime.UtcNow;
            sprint.Status = SprintStatus.Completed;
            await _sprintRepository.UpdateAsync(sprint);
            var projectMembers = await _projectMemberRepository.GetAllAsync(sprint.ProjectId);
            foreach (var member in projectMembers)
            {
                await _notificationService.SendNotificationAsync(
                    member.UserId,
                    $"Sprint '{sprint.Name}' has been completed",
                    NotificationType.SprintCompleted,
                    null
                );
            }
            return new BaseResponse
            {
                Success = true,
                Message = "Sprint end successfully"
            };
        }
    }
}