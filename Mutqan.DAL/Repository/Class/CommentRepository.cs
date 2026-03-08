using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<Comment?> FindByIdAsync(Guid commentId)
        {
            return await _context.Comments
                .Include(c => c.Task)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId && !c.IsDeleted);
        }
        public async Task<List<Comment>> GetAllAsync(Guid taskId)
        {
            return await _context.Comments
                .Include(c=>c.Task)
                .Include(c=>c.User)
                .Where(c => c.TaskId == taskId && !c.IsDeleted)
                .ToListAsync();
        }
    }
}
