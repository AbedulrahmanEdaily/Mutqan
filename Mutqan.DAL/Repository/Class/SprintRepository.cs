using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;


namespace Mutqan.DAL.Repository.Class
{
    public class SprintRepository : GenericRepository<Sprint>, ISprintRepository
    {
        private readonly AppDbContext _context;

        public SprintRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Sprint?> FindByIdAsync(Guid sprintId)
        {
            return await _context.Sprints
                .Include(s=>s.Tasks.Where(t => !t.IsDeleted))
                .Include(s=>s.Project)
                .FirstOrDefaultAsync(s => s.Id == sprintId && !s.IsDeleted);
        }
        public async Task<List<Sprint>> GetAllAsync(Guid projectId)
        {
            return await _context.Sprints
                .Include(s=>s.Tasks.Where(t => !t.IsDeleted))
                .Include(s=>s.Project)
                .Where(s => s.ProjectId == projectId && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> HasProjectActiveSprintAsync(Guid projectId)
        {
            return await _context.Sprints.AnyAsync(
                s => s.ProjectId == projectId 
                && s.Status == SprintStatus.Active 
                && !s.IsDeleted
            );
        }
    }
}
