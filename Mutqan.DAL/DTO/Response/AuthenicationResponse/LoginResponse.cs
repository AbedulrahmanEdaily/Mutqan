using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.AuthenicationResponse
{
    public class LoginResponse:BaseResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
