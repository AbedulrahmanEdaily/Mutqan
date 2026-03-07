using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class CreateTaskRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        [Required]
        public DateTime EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
    }
}
