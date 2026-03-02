using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.ProjectRequest
{
    public class CreateProjectRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [MinLength(10)]
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public Guid OrganizationId { get; set; }
    }
}
