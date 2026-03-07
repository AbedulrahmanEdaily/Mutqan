using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class ChangeTaskPriorityRequest
    {
        [Required]
        [Range(0,3)]
        public TaskPriority Priority { get; set; }
    }
}
