using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IProjectTaskRepository:IGenericRepository<ProjectTask> ,IScopedRepository
    {
        Task MoveUncompletedTasksToBacklogAsync(Guid sprintId);
    }
}
