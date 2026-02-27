using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Repository.Interface
{
    public interface IOrganizationRepository : IScopedRepository, IGenericRepository<Organization>
    {
        Task<Organization?> FindByIdAsync(Guid id);
    }
}
