using Mapster;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using Mutqan.DAL.DTO.Response.ProjectResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
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
        }
    }
}
