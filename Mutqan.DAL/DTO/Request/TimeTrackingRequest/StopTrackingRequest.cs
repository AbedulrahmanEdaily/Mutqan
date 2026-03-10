using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.TimeTrackingRequest
{
    public class StopTrackingRequest
    {
        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
