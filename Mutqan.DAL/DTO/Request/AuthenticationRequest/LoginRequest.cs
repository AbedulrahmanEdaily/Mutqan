using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Request.AuthenticationRequest
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
