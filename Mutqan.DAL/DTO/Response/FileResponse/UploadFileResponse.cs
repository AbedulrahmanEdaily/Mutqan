using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.FileResponse
{
    public class UploadFileResponse : BaseResponse
    {
        public string? FileUrl { get; set; }
        public string? PublicId { get; set; }
    }
}
