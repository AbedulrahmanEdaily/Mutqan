using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.FileResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Class
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ICloudinary _cloudinary;
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;

        public AttachmentService(
             IAttachmentRepository attachmentRepository
            ,ICloudinary cloudinary
            ,IProjectTaskRepository projectTaskRepository
            ,IProjectMemberRepository projectMemberRepository
            )
        {
            _attachmentRepository = attachmentRepository;
            _cloudinary = cloudinary;
            _projectTaskRepository = projectTaskRepository;
            _projectMemberRepository = projectMemberRepository;
        }
        public async Task<UploadFileResponse> AddAttachmentAsync(string userId, Guid taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new UploadFileResponse
                {
                    Success = false,
                    Message = "File is empty"
                };
            }
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if(task is null)
            {
                return new UploadFileResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, userId);
            var isAssignedDeveloper = task.AssignedToUserId == userId;
            if (!isProjectManager && !isAssignedDeveloper)
            {
                return new UploadFileResponse 
                { 
                    Success = false, 
                    Message = "User not allowed" 
                };
            }
            var result = await UploadFileAsync(file);
            var attachment = new Attachment
            {
                TaskId = taskId,
                UploadedByUserId = userId,
                FileName = result.DisplayName,
                FileUrl = result.SecureUrl.ToString(),
                PublicId = result.PublicId,
                FileType = Path.GetExtension(file.FileName),
                FileSize = file.Length
            };
            await _attachmentRepository.CreateAsync(attachment);
            return new UploadFileResponse
            {
                Success = true,
                FileUrl = result.SecureUrl.ToString(),
                PublicId = result.PublicId,
                Message = "File uploaded successfully"
            };
        }
        public async Task<BaseResponse> DeleteAttachmentAsync(string userId, Guid attachmentId)
        {
            var attachment = await _attachmentRepository.FindByIdAsync(attachmentId);
            if(attachment is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Attachment not found"
                };
            }
            var task = await _projectTaskRepository.GetTaskAsync(attachment.TaskId);
            if (task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, userId);
            var isAssignedDeveloper = attachment.UploadedByUserId == userId;
            if (!isProjectManager && !isAssignedDeveloper)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var deleteParams = new DeletionParams(attachment.PublicId);
            await _cloudinary.DestroyAsync(deleteParams);
            await _attachmentRepository.RemoveAsync(attachment);
            return new BaseResponse
            {
                Success = true,
                Message = "File deleted successfully"
            };
        }
        public async Task<List<AttachmentResponse>> GetTaskAttachmentsAsync(string adminId, Guid taskId)
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
            var attachments = await _attachmentRepository.GetAllAsync(taskId);
            return attachments.Adapt<List<AttachmentResponse>>();
        }
        private async Task<ImageUploadResult> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new AutoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "mutqan/attachments"
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            return result;
        }
    }
}
