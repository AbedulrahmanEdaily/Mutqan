using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface INotificationRepository : IScopedRepository
    {
        public Task CreateAsync(Notification notification);
        public Task UpdateAsync(Notification notification);
        public Task DeleteAsync(Notification notification);
        public Task<List<Notification>> GetAllAsync(string userId);
        public Task<Notification?> FindByIdAsync(Guid notificationId);
    }
}
