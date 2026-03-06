using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public enum SprintStatus
    {
        Planning,
        Active,
        Completed
    }
    public class Sprint:BaseModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string? Goal { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public SprintStatus Status { get; set; } = SprintStatus.Planning;
        public Project Project { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
