using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface ISprintRepository:IGenericRepository<Sprint> ,IScopedRepository
    {
        Task<bool> HasProjectActiveSprintAsync(Guid projectId);
        Task<Sprint?> FindByIdAsync(Guid sprintId);
        Task<List<Sprint>> GetAllAsync(Guid projectId);
    }
}
