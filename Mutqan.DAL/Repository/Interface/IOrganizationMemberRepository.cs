using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IOrganizationMemberRepository:IScopedRepository
    {
        Task<bool> IsAdminAsync(string userId,Guid organizationId);
        Task<bool> IsUserInOrganizationAsync(string userId);
        Task CreateAsync(OrganizationMember organizationMember);
        Task UpdateAsync(OrganizationMember organizationMember);
        Task DeleteAsync(OrganizationMember organizationMember);
        Task<OrganizationMember?> GetByUserIdAsync(string userId);
        Task<List<OrganizationMember>> GetByOrganizationIdAsync(Guid organizationId);
    }
}
