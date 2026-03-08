using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.TaskResponse
{
    public class TaskDependencyResponse
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public Guid DependsOnTaskId { get; set; }
        public string DependsOnTitle { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
