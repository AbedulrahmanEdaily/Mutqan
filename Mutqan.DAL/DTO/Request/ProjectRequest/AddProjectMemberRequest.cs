using Mutqan.DAL.Models;
using System.ComponentModel.DataAnnotations;


namespace Mutqan.DAL.DTO.Request.ProjectRequest
{
    public class AddProjectMemberRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public string UserId { get; set; }
        public ProjectRole Role { get; set; } = ProjectRole.Developer;
    }
}
