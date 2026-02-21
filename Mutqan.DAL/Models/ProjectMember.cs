using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public enum ProjectRole
    {
        ProjectManager,
        Developer,
        Client
    }
    public class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public Project Project { get; set; }
        public ApplicationUser User { get; set; }
    }
}
