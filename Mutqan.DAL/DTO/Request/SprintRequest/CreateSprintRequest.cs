using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.SprintRequest
{
    public class CreateSprintRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [MinLength(10)]
        public string? Goal { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
