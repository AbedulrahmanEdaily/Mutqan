using Mutqan.DAL.DTO.Request.CommentRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.CommentResponse;

namespace Mutqan.BLL.Services.Interface
{
    public interface ICommentService : IScopedService
    {
        Task<BaseResponse> AddCommentAsync(string requesterId, AddCommentRequest request);
        Task<BaseResponse> EditCommentAsync(string requesterId, Guid commentId, EditCommentRequest request);
        Task<BaseResponse> DeleteCommentAsync(string requesterId, Guid commentId);
        Task<List<CommentsResponse>> GetTaskCommentsAsync(string requesterId, Guid taskId);
    }
}
