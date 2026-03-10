using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IProjectTaskRepository:IGenericRepository<ProjectTask> ,IScopedRepository
    {
        Task MoveUncompletedTasksToBacklogAsync(Guid sprintId);
        Task<ProjectTask?> GetTaskAsync(Guid taskId);
        Task<List<ProjectTask>> GetAllAsync(Guid projectId, int limit, int page);
        Task<ProjectTask?> GetTaskDetailsAsync(Guid taskId);
    }
}
