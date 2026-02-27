

namespace Mutqan.DAL.Models
{
    public enum OrganizationRole
    {
        Admin,
        Member
    }
    public class OrganizationMember
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string UserId { get; set; }
        public OrganizationRole Role { get; set; } = OrganizationRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Organization Organization { get; set; }
        public ApplicationUser User { get; set; }
    }
}
