using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class TimeTracking:BaseModel
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public string? Notes { get; set; }
        public ProjectTask Task { get; set; }
        public ApplicationUser User { get; set; }
    }
}
