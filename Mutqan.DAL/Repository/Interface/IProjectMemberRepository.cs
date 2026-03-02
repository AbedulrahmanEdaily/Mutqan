using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IProjectMemberRepository:IScopedRepository
    {
        Task<bool> IsProjectManagerAsync(Guid projectId, string memberId);
        Task<bool> isProjectMemberAsync(Guid projectId,string memberId);
        Task<ProjectMember?> GetByUserIdAsync(string userId);
    }
}
