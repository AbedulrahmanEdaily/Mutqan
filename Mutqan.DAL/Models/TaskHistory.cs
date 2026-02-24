using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string ChangedByUserId { get; set; }
        public string FieldChanged { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public ProjectTask Task { get; set; }
        public ApplicationUser ChangedBy { get; set; }
    }
}

