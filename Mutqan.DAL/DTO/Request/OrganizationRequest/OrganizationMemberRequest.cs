using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.OrganizationRequest
{
    public class OrganizationMemberRequest
    {
        [Required]
        public Guid OrganizationId { get; set; }
        [Required]
        public string UserId { get; set; }
        public OrganizationRole Role { get; set; } = OrganizationRole.Member;
    }
}
