using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Request.OrganizationRequest
{
    public class OrganizationMemberRequest
    {
        public Guid OrganizationId { get; set; }
        public string UserId { get; set; }
        public OrganizationRole Role { get; set; } = OrganizationRole.Member;
    }
}
