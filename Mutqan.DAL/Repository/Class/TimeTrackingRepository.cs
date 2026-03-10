using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class TimeTrackingRepository : GenericRepository<TimeTracking>, ITimeTrackingRepository
    {
        private readonly AppDbContext _context;

        public TimeTrackingRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<TimeTracking?> FindByIdAsync(Guid timeTrackingId)
        {
            return await _context.TimeTrackings
                .Include(t=>t.Task)
                .Include(t=>t.User)
                .FirstOrDefaultAsync(t => t.Id == timeTrackingId && !t.IsDeleted);
        }

        public async Task<List<TimeTracking>> GetAllAsync(Guid taskId)
        {
            return await _context.TimeTrackings
                .Include(t => t.Task)
                .Include(t => t.User)
                .Where(t => t.TaskId == taskId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> HasDeveloperActiveTimeTracking(string userId)
        {
            return await _context.TimeTrackings
                .AnyAsync(
                    t => t.UserId == userId 
                    && t.EndTime == null 
                    && !t.IsDeleted
                );
        }
    }
}
