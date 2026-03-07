using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum TaskStatus
    {
        Backlog,
        Todo,
        InProgress,
        Review,
        Done
    }
    public class ProjectTask:BaseModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? SprintId { get; set; }
        public string? AssignedToUserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.Backlog;
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public Project Project { get; set; }
        public Sprint? Sprint { get; set; }
        public ApplicationUser? AssignedTo { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<TimeTracking> TimeTrackings { get; set; }
        public ICollection<TaskHistory> History { get; set; }
        public ICollection<TaskDependency> Dependencies { get; set; }
        public ICollection<TaskDependency> DependentOn { get; set; }
    }
}
