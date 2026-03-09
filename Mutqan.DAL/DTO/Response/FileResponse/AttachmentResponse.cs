using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.FileResponse
{
    public class AttachmentResponse
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string UploadedByUserId { get; set; }
        public string UploadedByFullName { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
