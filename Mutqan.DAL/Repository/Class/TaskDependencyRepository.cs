using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class TaskDependencyRepository : ITaskDependencyRepository
    {
        private readonly AppDbContext _context;

        public TaskDependencyRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Guid taskId, Guid dependsOnTaskId)
        {
            var deleted = await _context.TaskDependencies
                .FirstOrDefaultAsync(d => d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId && d.IsDeleted);
            if(deleted is not null)
            {
                deleted.IsDeleted = false;
                deleted.DeletedAt = null;
                _context.TaskDependencies.Update(deleted);
            }
            else
            {
                var taskDependency = new TaskDependency
                {
                    TaskId = taskId,
                    DependsOnTaskId = dependsOnTaskId
                };
                await _context.TaskDependencies.AddAsync(taskDependency);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasUncompletedDependenciesAsync(Guid taskId)
        {
            return await _context.TaskDependencies.AnyAsync(
                d => d.TaskId == taskId 
                && d.DependsOn.Status != DAL.Models.TaskStatus.Done
            );
        }
        public async Task<bool> IsDependencyExistsAsync(Guid taskId, Guid dependsOnTaskId)
        {
            return await _context.TaskDependencies
                .AnyAsync(d => d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId && !d.IsDeleted);
        }
        public async Task<bool> HasCircularDependencyAsync(Guid taskId, Guid dependsOnTaskId)
        {
            return await _context.TaskDependencies
                .AnyAsync(d => d.TaskId == dependsOnTaskId && d.DependsOnTaskId == taskId && !d.IsDeleted);
        }
        public async Task<bool> RemoveAsync(Guid taskId, Guid dependsOnTaskId)
        {
            var taskDependency = await _context.TaskDependencies.FirstOrDefaultAsync(d => d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId && !d.IsDeleted);
            if(taskDependency is null)
            {
                return false;
            }
            taskDependency.IsDeleted = true;
            taskDependency.DeletedAt = DateTime.UtcNow;
            _context.TaskDependencies.Update(taskDependency);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<TaskDependency>> GetAllAsync(Guid taskId)
        {
            return await _context.TaskDependencies.Include(d => d.DependsOn).Where(d => d.TaskId == taskId && !d.IsDeleted).ToListAsync();
        }
    }
}
