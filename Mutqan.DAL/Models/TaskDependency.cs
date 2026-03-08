using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    [PrimaryKey(nameof(TaskId),nameof(DependsOnTaskId))]
    public class TaskDependency
    {
        public Guid TaskId { get; set; }
        public Guid DependsOnTaskId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ProjectTask Task { get; set; }
        public ProjectTask DependsOn { get; set; }
    }
}
