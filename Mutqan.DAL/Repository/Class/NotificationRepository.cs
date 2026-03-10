using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Notification notification)
        {
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            notification.IsDeleted = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification?> FindByIdAsync(Guid notificationId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Task)
                .FirstOrDefaultAsync(n => n.Id == notificationId && !n.IsDeleted);
        }

        public async Task<List<Notification>> GetAllAsync(string userId, int limit = 3, int page = 1)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Task)
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
