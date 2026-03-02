using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.ProjectRequest
{
    public class ChangeProjectStatusRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        [Range(0,3)]
        public ProjectStatus Status { get; set; }
    }
}
