using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IProjectRepository:IGenericRepository<Project> , IScopedRepository
    {
        Task<Project?> FindByIdAsync(Guid projectId);
        Task<List<Project>> GetAllForOrganizationAdminAsync(Guid organizationId);
        Task<List<Project>> GetAllForProjectMembersAsync(string userId);
    }
}
