using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface ICommentRepository : IGenericRepository<Comment> , IScopedRepository
    {
        Task<Comment?> FindByIdAsync(Guid commentId);
        Task<List<Comment>> GetAllAsync(Guid taskId);
    }
}
