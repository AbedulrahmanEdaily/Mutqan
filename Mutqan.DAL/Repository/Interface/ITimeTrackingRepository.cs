using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface ITimeTrackingRepository : IGenericRepository<TimeTracking>,IScopedRepository
    {
        Task<TimeTracking?> FindByIdAsync(Guid timeTrackingId);
        Task<bool> HasDeveloperActiveTimeTracking(string userId);
        Task<List<TimeTracking>> GetAllAsync(Guid taskId);
    }
}
