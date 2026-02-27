using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class UserOrganizationHistory
    {
        public Guid Id { get; set; }
        public Guid OrganizationMemberId { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }
        public Organization Organization { get; set; }
        public OrganizationMember OrganizationMember { get; set; } 
    }
}
