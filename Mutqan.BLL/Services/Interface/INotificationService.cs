using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.NotificationResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface INotificationService : IScopedService
    {
        Task SendNotificationAsync(string userId, string message, NotificationType type, Guid? taskId = null);
        Task<List<GetAllNotificationResponse>> GetMyNotificationsAsync(string requesterId);
        Task<BaseResponse> MarkAsReadAsync(string requesterId, Guid notificationId);
        Task<BaseResponse> DeleteNotificationAsync(string requesterId, Guid notificationId);
    }
}
