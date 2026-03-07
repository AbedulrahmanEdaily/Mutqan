using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CodeResetPassword { get; set; }
        public DateTime? CodeResetPasswordExpiration { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<OrganizationMember> OrganizationMembers { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<TimeTracking> TimeTrackings { get; set; }
    }
}
