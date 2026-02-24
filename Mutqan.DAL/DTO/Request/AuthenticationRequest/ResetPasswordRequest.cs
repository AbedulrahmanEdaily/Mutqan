using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Request.AuthenticationRequest
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public string ResetCode { get; set; }
        public string Email { get; set; }
    }
}
