using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class OrganizationMemberRepository : IOrganizationMemberRepository
    {
        private readonly AppDbContext _context;

        public OrganizationMemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(OrganizationMember organizationMember)
        {
            await _context.AddAsync(organizationMember);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrganizationMember organizationMember)
        {
            organizationMember.IsDeleted = true;
            _context.Update(organizationMember);
            await _context.SaveChangesAsync();
        }
        public async Task<List<OrganizationMember>> GetByOrganizationIdAsync(Guid organizationId)
        {
            return await _context.OrganizationMembers
                .Include(x => x.User)
                .Include(x => x.Organization)
                .Where(x => x.OrganizationId == organizationId && !x.IsDeleted)
                .ToListAsync();
        }
        public async Task<OrganizationMember?> GetByUserIdAsync(string userId)
        {
            return await _context.OrganizationMembers
                .Include(m => m.Organization)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId && !m.IsDeleted);
        }

        public async Task<bool> IsAdminAsync(string userId, Guid organizationId)
        {
            return await _context.OrganizationMembers
                .AnyAsync(m => m.UserId == userId
                      &&m.OrganizationId == organizationId
                      && m.Role == OrganizationRole.Admin
                      && !m.IsDeleted);
        }

        public async Task<bool> IsUserInOrganizationAsync(string userId)
        {
            return await _context.OrganizationMembers
                .AnyAsync(m => m.UserId == userId && !m.IsDeleted);
        }

        public async Task UpdateAsync(OrganizationMember organizationMember)
        {
            _context.Update(organizationMember);
            await _context.SaveChangesAsync();
        }
    }
}
