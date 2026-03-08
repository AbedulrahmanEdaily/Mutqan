using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutqan.DAL.DTO.Request.UserRequest
{
    public class ChangeUserRoleRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
