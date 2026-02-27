using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class UserOrgainzatinHistoryRepository : IUserOrgainzatinHistoryRepository
    {
        private readonly AppDbContext _context;

        public UserOrgainzatinHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserOrganizationHistory userOrganizationHistory)
        {
            await _context.UserOrganizationHistories.AddAsync(userOrganizationHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<UserOrganizationHistory?> GetActiveByOrganizationMemberIdAsync(Guid organizationMemberId)
        {
            return await _context.UserOrganizationHistories
                .FirstOrDefaultAsync(h =>
                    h.OrganizationMemberId == organizationMemberId &&
                    h.LeftAt == null);
        }

        public async Task UpdateAsync(UserOrganizationHistory userOrganizationHistory)
        {
            _context.UserOrganizationHistories.Update(userOrganizationHistory);
            await _context.SaveChangesAsync();
        }
    }
}
