using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.CommentResponse
{
    public class CommentsResponse
    {
        public Guid CommentId { get; set; }
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
    }
}
