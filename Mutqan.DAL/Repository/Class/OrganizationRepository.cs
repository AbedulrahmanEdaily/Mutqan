using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Data;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Class
{
    public class OrganizationRepository :GenericRepository<Organization>, IOrganizationRepository
    {
        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<Organization?> FindByIdAsync(Guid id)
        {
            return await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        }
    }
}
