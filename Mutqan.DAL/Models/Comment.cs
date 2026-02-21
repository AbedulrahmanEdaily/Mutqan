using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class Comment : BaseModel
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public Task Task { get; set; }
        public ApplicationUser User { get; set; }
    }
}
