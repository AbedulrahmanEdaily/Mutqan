using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class AddTaskToSprintRequest
    {
        [Required]
        public Guid SprintId { get; set; }
        [Required]
        public Guid TaskId { get; set; }
    }
}
