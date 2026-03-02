using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.AuthenticationRequest
{
    public class ResetPasswordRequest
    {
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string ResetCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
