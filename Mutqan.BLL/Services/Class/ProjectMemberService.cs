using Mapster;
using Microsoft.AspNetCore.Identity;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.ProjectResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
namespace Mutqan.BLL.Services.Class
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public ProjectMemberService(IProjectMemberRepository projectMemberRepository
            , UserManager<ApplicationUser> userManager
            ,IProjectRepository projectRepository
            ,IOrganizationMemberRepository organizationMemberRepository
            )
        {
            _projectMemberRepository = projectMemberRepository;
            _userManager = userManager;
            _projectRepository = projectRepository;
            _organizationMemberRepository = organizationMemberRepository;
        }
        public async Task<BaseResponse> AddUserToProjectAsync(string adminId, AddProjectMemberRequest request)
        {
            var project = await _projectRepository.FindByIdAsync(request.ProjectId);
            if (project is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project not found"
                };
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(project.Id, adminId);
            if(!isOrganizationAdmin && !isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var organizationMember = await _organizationMemberRepository.GetByUserIdAsync(user.Id);
            if(organizationMember is null || organizationMember.OrganizationId != project.OrganizationId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User is not in the same organization"
                };
            }
            if (request.Role == ProjectRole.ProjectManager)
            {
                var hasProjectManager = await _projectMemberRepository.IsProjectHasManagerAsync(request.ProjectId);
                if (hasProjectManager)
                {
                    return new BaseResponse {
                        Success = false,
                        Message = "Project already has a manager"
                    };
                }
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(project.Id, user.Id);
            if (isProjectMember)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User is already a project member"
                };
            }
            var result = request.Adapt<ProjectMember>();
            await _projectMemberRepository.AddAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Project member added successfully"
            };
        }
        public async Task<List<ProjectMemberResponse>> GetAllProjectMembersAsync(string adminId,Guid projectId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if(project is null)
            {
                return [];
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(projectId, adminId);
            if (!isProjectMember&&!isOrganizationAdmin)
            {
                return [];
            }
            var members = await _projectMemberRepository.GetAllAsync(projectId);
            return members.Adapt<List<ProjectMemberResponse>>();
        }
        public async Task<ProjectMemberResponse?> GetProjectMemberByIdAsync(string adminId, Guid projectMemberId)
        {
            var member = await _projectMemberRepository.GetByProjectMemberIdAsync(projectMemberId);
            if (member is null) 
            { 
                return null; 
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, member.Project.OrganizationId);
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(member.ProjectId, adminId);

            if (!isOrganizationAdmin && !isProjectMember)
            {
                return null;
            }
            return member.Adapt<ProjectMemberResponse>();
        }
        public async Task<BaseResponse> RemoveUserFromProjectAsync(string adminId, Guid projectId, string userId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if (project is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project not found"
                };
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(project.Id, adminId);
            if (!isOrganizationAdmin && !isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (adminId == userId)
            {
                return new BaseResponse {
                    Success = false,
                    Message = "Can't remove yourself from project"
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var projectMember = await _projectMemberRepository.GetByUserIdAndProjectIdAsync(projectId,userId);
            if(projectMember is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project member not found"
                };
            }
            await _projectMemberRepository.RemoveAsync(projectMember);
            return new BaseResponse
            {
                Success = true,
                Message = "Project member removed successfully"
            };
        }
    }
}
