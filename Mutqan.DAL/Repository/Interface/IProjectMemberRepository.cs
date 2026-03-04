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
        Task<bool> IsProjectHasManagerAsync(Guid projectId);
        Task<ProjectMember?> GetByProjectMemberIdAsync(Guid projectMemberId);
        Task<ProjectMember?> GetByUserIdAsync(string userId);
        Task<ProjectMember?> GetByUserIdAndProjectIdAsync(Guid projectId,string userId);
        Task<List<ProjectMember>> GetAllAsync(Guid projectId);
        Task AddAsync(ProjectMember member);
        Task RemoveAsync(ProjectMember member);
    }
}
