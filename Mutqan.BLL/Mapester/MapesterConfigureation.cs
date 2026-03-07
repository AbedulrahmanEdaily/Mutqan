using Mapster;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Request.SprintRequest;
using Mutqan.DAL.DTO.Request.TaskRequest;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using Mutqan.DAL.DTO.Response.ProjectResponse;
using Mutqan.DAL.DTO.Response.SprintResponse;
using Mutqan.DAL.DTO.Response.TaskResponse;
using Mutqan.DAL.DTO.Response.UserResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Mutqan.BLL.Mapester
{
    public static class MapesterConfigureation
    {
        public static void Configureation()
        {
            TypeAdapterConfig<OrganizationRequest, Organization>.NewConfig()
                .IgnoreNullValues(true);
            TypeAdapterConfig<OrganizationMember, OrganizationMemberResponse>.NewConfig()
                .Map(dest => dest.FullName, src => src.User.FullName)
                .Map(dest=>dest.OrganizationName , src => src.Organization.Name);
            TypeAdapterConfig<UpdateProjectRequest, Project>.NewConfig()
                .IgnoreNullValues(true);
            TypeAdapterConfig<ProjectMember, ProjectMemberResponse>.NewConfig()
                .Map(dest => dest.ProjectMemberId, src => src.Id)
                .Map(dest => dest.FullName, src => src.User.FullName);
            TypeAdapterConfig<CreateSprintRequest, Sprint>.NewConfig()
                .Map(dest => dest.EstimatedStartDate, src => src.StartDate)
                .Map(dest => dest.EstimatedEndDate, src => src.EndDate);
            TypeAdapterConfig<UpdateSprintRequest, Sprint>.NewConfig()
                .IgnoreNullValues(true);
            TypeAdapterConfig<Sprint, SprintResponse>.NewConfig()
                .Map(src =>src.SprintId, dest =>dest.Id)
                .Map(src =>src.SprintName, dest =>dest.Name);
            TypeAdapterConfig<Sprint, SprintDetailsResponse>.NewConfig()
                .Map(src =>src.SprintId, dest =>dest.Id)
                .Map(src =>src.SprintName, dest =>dest.Name)
                ;
            TypeAdapterConfig<UpdateTaskRequest, ProjectTask>.NewConfig()
                .IgnoreNullValues(true);
            TypeAdapterConfig<ProjectTask,ProjectTaskResponse>.NewConfig()
                .Map(src=>src.TaskId,dest=>dest.Id);
            TypeAdapterConfig<ProjectTask,TaskDetailsResponse>.NewConfig()
                .Map(src=>src.TaskId,dest=>dest.Id)
                .Map(src=>src.AssignedToFullName,dest=>dest.AssignedTo.FullName);
        }
    }
}
