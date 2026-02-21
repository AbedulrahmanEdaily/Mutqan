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
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public SprintStatus Status { get; set; } = SprintStatus.Planning;
        public Project Project { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
