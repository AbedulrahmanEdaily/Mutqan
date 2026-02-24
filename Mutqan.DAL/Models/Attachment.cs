using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string UploadedByUserId { get; set; }
        public string FileName { get; set; }
        public string FileUrl{ get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ProjectTask Task { get; set; }
        public ApplicationUser UploadedBy { get; set; }
    }
}
