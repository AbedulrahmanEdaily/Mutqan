using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
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
        public async Task<bool> HasUncompletedDependenciesAsync(Guid taskId)
        {
            return await _context.TaskDependencies.AnyAsync(
                d => d.TaskId == taskId 
                && d.DependsOn.Status != DAL.Models.TaskStatus.Done
            );
        }
    }
}
