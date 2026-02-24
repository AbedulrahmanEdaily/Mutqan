using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Request.AuthenticationRequest
{
    public class RefreshTokenRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
