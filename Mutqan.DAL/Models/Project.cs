using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public enum ProjectStatus
    {
        Active,
        OnHold,
        Completed,
        Archived
    }
    public class Project:BaseModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
        public Organization Organization { get; set; }
        public ICollection<ProjectMember> Members { get; set; }
        public ICollection<Sprint> Sprints { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
