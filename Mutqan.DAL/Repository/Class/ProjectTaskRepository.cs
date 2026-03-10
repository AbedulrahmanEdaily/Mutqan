using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class ProjectTaskRepository : GenericRepository<ProjectTask>, IProjectTaskRepository
    {
        private readonly AppDbContext _context;

        public ProjectTaskRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<List<ProjectTask>> GetAllAsync(Guid projectId, int limit, int page )
        {
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.Sprint)
                .Where(t => t.ProjectId == projectId && !t.IsDeleted)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        public async Task<ProjectTask?> GetTaskAsync(Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);
        }
        public async Task<ProjectTask?> GetTaskDetailsAsync(Guid taskId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .Include(t => t.Sprint)
                .Include(t => t.Comments.Where(c => !c.IsDeleted))
                .Include(t => t.Attachments.Where(a => !a.IsDeleted))
                .Include(t => t.Dependencies.Where(d => !d.IsDeleted))
                .Where(t => t.Id == taskId && !t.IsDeleted)
                .FirstOrDefaultAsync();
        }
        public async Task MoveUncompletedTasksToBacklogAsync(Guid sprintId)
        {
            var tasks = await _context.Tasks
                .Where(
                    t => t.SprintId == sprintId 
                    && (t.Status == Models.TaskStatus.Todo || t.Status == Models.TaskStatus.InProgress || t.Status == Models.TaskStatus.Review) 
                    && !t.IsDeleted
                )
                .ToListAsync();

            foreach(var task in tasks)
            {
                task.Status = Models.TaskStatus.Backlog;
                task.SprintId = null;
            }
            _context.UpdateRange(tasks);
            await _context.SaveChangesAsync();
        }
    }
}
