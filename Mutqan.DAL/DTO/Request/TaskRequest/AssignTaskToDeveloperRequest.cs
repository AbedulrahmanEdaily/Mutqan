using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class AssignTaskToDeveloperRequest
    {
        [Required]
        public string DeveloperId { get; set; }
    }
}
