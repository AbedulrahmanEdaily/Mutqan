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

        public async Task AddAsync(ProjectMember member)
        {
            await _context.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectMember>> GetAllAsync(Guid projectId)
        {
            return await _context.ProjectMembers
                .Include(m => m.Project)
                .Include(m => m.User)
                .Where(m => m.ProjectId == projectId && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetByProjectMemberIdAsync(Guid projectMemberId)
        {
            return await _context.ProjectMembers
                .Include(m => m.User)
                .Include(m => m.Project)
                .FirstOrDefaultAsync(
                    m => m.Id == projectMemberId
                    && !m.IsDeleted
                );
        }

        public async Task<ProjectMember?> GetByUserIdAndProjectIdAsync(Guid projectId, string userId)
        {
            return await _context.ProjectMembers
               .Include(m => m.Project)
               .Include(m => m.User)
               .FirstOrDefaultAsync(
                   m => m.UserId == userId
                   && m.ProjectId == projectId
                   && !m.IsDeleted
               );
        }

        public async Task<ProjectMember?> GetByUserIdAsync(string userId)
        {
            return await _context.ProjectMembers
                .Include(m => m.User)
                .Include(m => m.Project)
                .FirstOrDefaultAsync(
                    m => m.UserId == userId 
                    && !m.IsDeleted
                );
        }

        public async Task<bool> IsProjectHasManagerAsync(Guid projectId)
        {
            return await _context.ProjectMembers
                .AnyAsync(
                    m => m.ProjectId == projectId 
                    && m.Role == ProjectRole.ProjectManager 
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

        public async Task RemoveAsync(ProjectMember member)
        {
            member.IsDeleted = true;
            _context.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
