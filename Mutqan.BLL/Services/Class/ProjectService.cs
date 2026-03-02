using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.ProjectResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Class;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Class
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectService(IProjectRepository projectRepository
            ,UserManager<ApplicationUser> userManager
            ,IOrganizationMemberRepository organizationMemberRepository
            ,IOrganizationRepository organizationRepository
            ,IProjectMemberRepository projectMemberRepository
            )
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
            _organizationMemberRepository = organizationMemberRepository;
            _organizationRepository = organizationRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public async Task<List<ProjectResponse>> GetAllProjectAsync(string adminId)
        {
            var organizationMember = await _organizationMemberRepository.GetByUserIdAsync(adminId);
            if(organizationMember is null)
            {
                return [];
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, organizationMember.OrganizationId);
            if (isOrganizationAdmin)
            {
                var projectsForOrganizationAdmin = await _projectRepository.GetAllForOrganizationAdminAsync(organizationMember.OrganizationId);
                return projectsForOrganizationAdmin.Adapt<List<ProjectResponse>>();
            }

            var projects = await _projectRepository.GetAllForProjectMembersAsync(adminId);
            return projects.Adapt<List<ProjectResponse>>();
        }
        public async Task<ProjectResponse?> GetProjectByIdAsync(string adminId,Guid projectId)
        {
            var project = await _projectRepository.FindByIdAsync(projectId);
            if(project is null)
            {
                return null;
            }
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(projectId,adminId);
            if(!isOrganizationAdmin && !isProjectMember)
            {
                return null;
            }
            return project.Adapt<ProjectResponse>();
        }
        public async Task<BaseResponse> CreateProjectAsync(string adminId, CreateProjectRequest request)
        {
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId,request.OrganizationId);
            if (!isOrganizationAdmin)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var organization = await _organizationRepository.FindByIdAsync(request.OrganizationId);
            if (organization is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }
            var result = request.Adapt<Project>();
            await _projectRepository.CreateAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Project created successfully"
            };
        }
        public async Task<BaseResponse> UpdateProjectAsync(string adminId, UpdateProjectRequest request)
        {
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(request.Id, adminId);
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, request.OrganizationId);
            if (!isOrganizationAdmin && !isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }

            var project = await _projectRepository.FindByIdAsync(request.Id);
            if(project is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Project not found"
                };
            }
            request.Adapt(project);
            await _projectRepository.UpdateAsync(project);
            return new BaseResponse
            {
                Success = true,
                Message = "Project updated successfully"
            };
        }
        public async Task<BaseResponse> DeleteProjectAsync(string adminId, Guid projectId)
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
            if (!isOrganizationAdmin)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            await _projectRepository.DeleteAsync(project);
            return new BaseResponse
            {
                Success = true,
                Message = "Project deleted successfully"
            };
            
        }
        public async Task<BaseResponse> ChangeProjectStatusAsync(string adminId,ChangeProjectStatusRequest request)
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
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(project.Id,adminId);
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(adminId, project.OrganizationId);
            if(!isOrganizationAdmin && !isProjectManager)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowd"
                };
            }
            project.Status = request.Status;
            await _projectRepository.UpdateAsync(project);
            return new BaseResponse
            {
                Success = true,
                Message = "Status changed successflly"
            };
        }

    }
}
