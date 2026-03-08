using Azure.Core;
using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.CommentRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.CommentResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Class
{
    public class CommentSerivce : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;

        public CommentSerivce(
             ICommentRepository commentRepository
            ,IProjectMemberRepository projectMemberRepository
            ,IProjectTaskRepository projectTaskRepository
            )
        {
            _commentRepository = commentRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectTaskRepository = projectTaskRepository;
        }
        public async Task<BaseResponse> AddCommentAsync(string adminId, AddCommentRequest request)
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
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, adminId);
            if (!isProjectMember)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var result = request.Adapt<Comment>();
            result.UserId = adminId;
            await _commentRepository.CreateAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Comment added successfully"
            };
        }
        public async Task<BaseResponse> EditCommentAsync (string adminId, Guid commentId, EditCommentRequest request)
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
            if (comment.UserId != adminId)
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
        public async Task<BaseResponse> DeleteCommentAsync(string adminId, Guid commentId)
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
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(comment.Task.ProjectId, adminId);
            if (comment.UserId != adminId && !isProjectManager)
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
        public async Task<List<CommentsResponse>> GetTaskCommentsAsync(string adminId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return [];
            }
            var isProjectMember = await _projectMemberRepository.isProjectMemberAsync(task.ProjectId, adminId);
            if (!isProjectMember)
            {
                return [];
            }
            var result = await _commentRepository.GetAllAsync(taskId);
            return result.Adapt<List<CommentsResponse>>();
        }
    }
}
