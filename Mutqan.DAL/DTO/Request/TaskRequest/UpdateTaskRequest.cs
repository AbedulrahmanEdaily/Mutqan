using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class UpdateTaskRequest
    {
        [MinLength(3)]
        [MaxLength(200)]
        public string? Title { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedDueDate { get; set; }
    }
}
