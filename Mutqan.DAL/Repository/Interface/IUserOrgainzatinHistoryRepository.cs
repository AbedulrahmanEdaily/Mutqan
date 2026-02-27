using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IUserOrgainzatinHistoryRepository:IScopedRepository
    {
        Task CreateAsync(UserOrganizationHistory userOrganizationHistory);
        Task UpdateAsync(UserOrganizationHistory userOrganizationHistory);
        Task<UserOrganizationHistory?> GetActiveByOrganizationMemberIdAsync(Guid organizationMemberId);
    }
}
