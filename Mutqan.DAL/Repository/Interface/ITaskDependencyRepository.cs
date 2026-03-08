using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface ITaskDependencyRepository : IScopedRepository
    {
        Task<bool> HasUncompletedDependenciesAsync(Guid taskId);
        Task<bool> IsDependencyExistsAsync(Guid taskId, Guid dependesOnTaskId);
        Task<bool> HasCircularDependencyAsync(Guid taskId, Guid dependsOnTaskId);
        Task CreateAsync(Guid taskId, Guid dependsOnTaskId);
        Task<bool> RemoveAsync(Guid taskId, Guid dependsOnTaskId);
        Task<List<TaskDependency>> GetAllAsync(Guid taskId);
    }
}
