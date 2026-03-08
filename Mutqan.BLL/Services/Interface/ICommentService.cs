using Mutqan.DAL.DTO.Request.CommentRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.CommentResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface ICommentService : IScopedService
    {
        Task<BaseResponse> AddCommentAsync(string adminId, AddCommentRequest request);
        Task<BaseResponse> EditCommentAsync(string adminId, Guid commentId, EditCommentRequest request);
        Task<BaseResponse> DeleteCommentAsync(string adminId, Guid commentId);
        Task<List<CommentsResponse>> GetTaskCommentsAsync(string adminId, Guid taskId);
    }
}
