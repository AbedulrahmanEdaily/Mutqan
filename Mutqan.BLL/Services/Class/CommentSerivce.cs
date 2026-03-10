using Azure.Core;
using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.CommentRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.CommentResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;

namespace Mutqan.BLL.Services.Class
{
    public class CommentSerivce : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly INotificationService _notificationService;

        public CommentSerivce(
             ICommentRepository commentRepository
            ,IProjectMemberRepository projectMemberRepository
            ,IProjectTaskRepository projectTaskRepository
            ,INotificationService notificationService
            )
        {
            _commentRepository = commentRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectTaskRepository = projectTaskRepository;
            _notificationService = notificationService;
        }
        public async Task<BaseResponse> AddCommentAsync(string requesterId, AddCommentRequest request)
        {
            var task = await _projectTaskRepository.GetTaskAsync(request.TaskId);
            if(task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, requesterId);
            if (!isProjectMember)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var result = request.Adapt<Comment>();
            result.UserId = requesterId;
            await _commentRepository.CreateAsync(result);
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, requesterId);
            var receiverId = isProjectManager ? task.AssignedToUserId : (await _projectMemberRepository.GetProjectManagerAsync(task.ProjectId))?.UserId;
            if (receiverId is not null && receiverId != requesterId)
                await _notificationService.SendNotificationAsync(
                    receiverId,
                    $"New comment added on task: {task.Title}",
                    NotificationType.CommentAdded,
                    task.Id
                );
            return new BaseResponse
            {
                Success = true,
                Message = "Comment added successfully"
            };
        }
        public async Task<BaseResponse> EditCommentAsync (string requesterId, Guid commentId, EditCommentRequest request)
        {
            var comment = await _commentRepository.FindByIdAsync(commentId);
            if (comment is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "comment not found"
                };
            }
            if (comment.UserId != requesterId)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "You can only edit your own comments" 
                };
            }
            request.Adapt(comment);
            await _commentRepository.UpdateAsync(comment);
            return new BaseResponse
            {
                Success = true,
                Message = "Comment edited successfully"
            };
        }
        public async Task<BaseResponse> DeleteCommentAsync(string requesterId, Guid commentId)
        {
            var comment = await _commentRepository.FindByIdAsync(commentId);
            if (comment is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "comment not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(comment.Task.ProjectId, requesterId);
            if (comment.UserId != requesterId && !isProjectManager)
            {
                return new BaseResponse 
                { 
                    Success = false, 
                    Message = "User not allowed" 
                };
            }
            await _commentRepository.DeleteAsync(comment);
            return new BaseResponse
            {
                Success = true,
                Message = "Comment deleted successfully"
            };
        }
        public async Task<List<CommentsResponse>> GetTaskCommentsAsync(string requesterId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return [];
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, requesterId);
            if (!isProjectMember)
            {
                return [];
            }
            var result = await _commentRepository.GetAllAsync(taskId);
            return result.Adapt<List<CommentsResponse>>();
        }
    }
}
