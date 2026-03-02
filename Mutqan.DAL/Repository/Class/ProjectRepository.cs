using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class ProjectRepository:GenericRepository<Project>,IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<Project?> FindByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Organization)
                .FirstOrDefaultAsync(p => p.Id == projectId && !p.IsDeleted);
        }

        public async Task<List<Project>> GetAllForOrganizationAdminAsync(Guid organizationId)
        {
            return await _context.Projects
                .Include(p => p.Organization)
                .Where(p => p.OrganizationId == organizationId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllForProjectMembersAsync(string userId)
        {
            return await _context.Projects
                .Include(p=>p.Organization)
                .Where(p => !p.IsDeleted
                    && p.Members.Any(m => m.UserId == userId && !m.IsDeleted))
                .ToListAsync();
        }
    }
}
