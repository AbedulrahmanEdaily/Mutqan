using Mapster;
using Microsoft.AspNetCore.SignalR;
using Mutqan.BLL.RealTime;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.NotificationResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Class
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IHubContext<NotificationHub> hubContext
            , INotificationRepository notificationRepository
            
            )
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        public async Task<BaseResponse> DeleteNotificationAsync(string requesterId, Guid notificationId)
        {
            var notification = await _notificationRepository.FindByIdAsync(notificationId);
            if (notification is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Notification not found"
                };
            }
            if (notification.UserId != requesterId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            await _notificationRepository.DeleteAsync(notification);
            return new BaseResponse
            {
                Success = true,
                Message = "Notification deleted"
            };
        }
        public async Task<List<GetAllNotificationResponse>> GetMyNotificationsAsync(string requesterId)
        {
            var notifications = await _notificationRepository.GetAllAsync(requesterId);
            return notifications.Adapt<List<GetAllNotificationResponse>>();
        }
        public async Task<BaseResponse> MarkAsReadAsync(string requesterId, Guid notificationId)
        {
            var notification = await _notificationRepository.FindByIdAsync(notificationId);
            if(notification is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Notification not found"
                };
            }
            if(notification.UserId != requesterId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return new BaseResponse
            {
                Success = true,
                Message = "Notification marked as read"
            };
        }
        public async Task SendNotificationAsync(string userId, string message, NotificationType type, Guid? taskId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                TaskId = taskId
            };
            await _notificationRepository.CreateAsync(notification);
            await _hubContext.Clients
                .Group(userId)
                .SendAsync("ReceiveNotification", message);
        }
    }
}
