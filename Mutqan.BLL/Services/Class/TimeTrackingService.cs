using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.TimeTrackingRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TimeTrackingResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Class
{
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTaskRepository _projectTaskRepository;

        public TimeTrackingService(
             ITimeTrackingRepository timeTrackingRepository
            ,IProjectMemberRepository projectMemberRepository
            ,IProjectTaskRepository projectTaskRepository
            )
        {
            _timeTrackingRepository = timeTrackingRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectTaskRepository = projectTaskRepository;
        }
        public async Task<List<GetTimeTrackingResponse>> GetTaskTimeTrackingsAsync(string requesterId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return [];
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, requesterId);
            var isDeveloper = task.AssignedToUserId == requesterId;
            if (!isProjectManager && !isDeveloper)
            {
                return [];
            }
            var trackings = await _timeTrackingRepository.GetAllAsync(taskId);
            return trackings.Adapt<List<GetTimeTrackingResponse>>();
        }
        public async Task<GetTotalTimeTrackingResponse?> GetTotalTimeAsync(string requesterId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if (task is null)
            {
                return null;
            }
            var isProjectManager = await _projectMemberRepository.IsProjectManagerAsync(task.ProjectId, requesterId);
            var isDeveloper = task.AssignedToUserId == requesterId;
            if (!isProjectManager && !isDeveloper)
            {
                return null;
            }
            var trackings = await _timeTrackingRepository.GetAllAsync(taskId);
            var response = new GetTotalTimeTrackingResponse
            {
                TimeTrackings = trackings.Select(t => new TimeTrackingSummaryResponse
                {
                    Id = t.Id,
                    UserFullName = t.User.FullName,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Duration = t.Duration,
                }
                ).ToList()
            };
            return response;
        }
        public async Task<BaseResponse> StartTrackingAsync(string requesterId, Guid taskId)
        {
            var task = await _projectTaskRepository.GetTaskAsync(taskId);
            if(task is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };
            }
            if (requesterId != task.AssignedToUserId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (task.Status != DAL.Models.TaskStatus.InProgress)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task must be in progress"
                };
            }
            if (await _timeTrackingRepository.HasDeveloperActiveTimeTracking(requesterId))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You already have an active time tracking session"
                };
            }
            var timeTracking = new TimeTracking
            {
                UserId = requesterId,
                TaskId = task.Id,
                StartTime = DateTime.UtcNow
            };
            await _timeTrackingRepository.CreateAsync(timeTracking);
            return new BaseResponse
            {
                Success = true,
                Message = "Time tracking started successfully"
            };
        }
        public async Task<BaseResponse> StopTrackingAsync(string requesterId, Guid timeTrackingId, StopTrackingRequest request)
        {
            var timeTracking = await _timeTrackingRepository.FindByIdAsync(timeTrackingId);
            if(timeTracking is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Time tracking not found"
                };
            }
            if (requesterId != timeTracking.UserId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if(timeTracking.EndTime != null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Tracking is already closed"
                };
            }
            timeTracking.EndTime = DateTime.UtcNow;
            timeTracking.Duration = (timeTracking.EndTime.Value - timeTracking.StartTime).TotalMinutes;
            timeTracking.Notes = request.Notes;
            await _timeTrackingRepository.UpdateAsync(timeTracking);
            return new BaseResponse
            {
                Success = true,
                Message = "Time tracking stopped successfully"
            };
        }
    }
}
