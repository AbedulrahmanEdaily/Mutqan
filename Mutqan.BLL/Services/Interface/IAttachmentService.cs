using Microsoft.AspNetCore.Http;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.FileResponse;


namespace Mutqan.BLL.Services.Interface
{
    public interface IAttachmentService : IScopedService
    {
        Task<UploadFileResponse> AddAttachmentAsync(string userId, Guid taskId, IFormFile file);
        Task<BaseResponse> DeleteAttachmentAsync(string userId, Guid attachmentId);
        Task<List<AttachmentResponse>> GetTaskAttachmentsAsync(string requesterId, Guid taskId);
    }
}
