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
