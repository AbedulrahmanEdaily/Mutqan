using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.SprintRequest
{
    public class UpdateSprintRequest
    {

        [MinLength(3)]
        public string Name { get; set; }
        [MinLength(10)]
        public string? Goal { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
