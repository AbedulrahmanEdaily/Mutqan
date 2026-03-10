using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.NotificationResponse;
using Mutqan.DAL.Models;

namespace Mutqan.BLL.Services.Interface
{
    public interface INotificationService : IScopedService
    {
        Task SendNotificationAsync(string userId, string message, NotificationType type, Guid? taskId = null);
        Task<PagintedResponse<GetAllNotificationResponse>> GetMyNotificationsAsync(string requesterId, int limit = 3, int page = 1);
        Task<BaseResponse> MarkAsReadAsync(string requesterId, Guid notificationId);
        Task<BaseResponse> DeleteNotificationAsync(string requesterId, Guid notificationId);
    }
}
