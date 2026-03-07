using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface ITaskDependencyRepository : IScopedRepository
    {
        Task<bool> HasUncompletedDependenciesAsync(Guid taskId);
    }
}
