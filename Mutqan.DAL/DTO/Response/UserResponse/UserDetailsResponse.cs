using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutqan.DAL.DTO.Response.UserResponse
{
    public class UserDetailsResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
