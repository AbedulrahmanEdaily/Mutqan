using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mutqan.DAL.DTO.Request.CommentRequest
{
    public class EditCommentRequest
    {
        [Required]
        [MinLength(5)]
        [MaxLength(2000)]
        public string Content { get; set; }
    }
}
