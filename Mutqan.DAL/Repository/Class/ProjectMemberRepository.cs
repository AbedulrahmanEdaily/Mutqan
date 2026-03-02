using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly AppDbContext _context;

        public ProjectMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectMember?> GetByUserIdAsync(string userId)
        {
            return await _context.ProjectMembers
                .Include(m => m.User)
                .FirstOrDefaultAsync(
                    m => m.UserId == userId 
                    && !m.IsDeleted
                );
        }

        public async Task<bool> IsProjectManagerAsync(Guid projectId, string memberId)
        {
            return await _context.ProjectMembers.AnyAsync(
                p => p.UserId == memberId
                && p.ProjectId == projectId
                && p.Role == ProjectRole.ProjectManager
                && !p.IsDeleted
            );
        }
        public async Task<bool> isProjectMemberAsync(Guid projectId, string memberId)
        {
            return await _context.ProjectMembers.AnyAsync(
               p => p.UserId == memberId
               && p.ProjectId == projectId
               && !p.IsDeleted
           );
        }
    }
}
