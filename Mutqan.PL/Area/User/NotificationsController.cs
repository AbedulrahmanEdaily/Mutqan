using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetMyNotifications([FromQuery] int limit = 3, [FromQuery]int page = 1)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.GetMyNotificationsAsync(requesterId, limit, page);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{notificationId}")]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid notificationId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.MarkAsReadAsync(requesterId, notificationId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] Guid notificationId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.DeleteNotificationAsync(requesterId, notificationId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
