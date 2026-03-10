using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.TimeTrackingResponse
{
    public class GetTimeTrackingResponse
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Duration { get; set; }
        public string? Notes { get; set; }
    }
}
