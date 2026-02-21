using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class Organization:BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<UserOrganizationHistory> OrganizationHistories { get; set; }
    }
}
