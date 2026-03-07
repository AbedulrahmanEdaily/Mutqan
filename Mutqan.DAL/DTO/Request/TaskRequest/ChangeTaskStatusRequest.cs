

using System.ComponentModel.DataAnnotations;

namespace Mutqan.DAL.DTO.Request.TaskRequest
{
    public class ChangeTaskStatusRequest
    {
        [Required]
        [Range(0,4)]
        public Models.TaskStatus Status { get; set; }
    }
}
