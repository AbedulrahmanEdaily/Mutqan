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
        public Task Task { get; set; }
        public Task DependsOn { get; set; }
    }
}
