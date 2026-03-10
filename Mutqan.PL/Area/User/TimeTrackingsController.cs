using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.TimeTrackingRequest;
using System.Security.Claims;


namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TimeTrackingsController : ControllerBase
    {
        private readonly ITimeTrackingService _timeTrackingService;

        public TimeTrackingsController(ITimeTrackingService timeTrackingService)
        {
            _timeTrackingService = timeTrackingService;
        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetAllTimeTrackings([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.GetTaskTimeTrackingsAsync(requesterId, taskId);
            return Ok(new
            {
                Success = true,
                Message = "Trackings retrieved successfully",
                Trackings = result
            });
        }
        [HttpGet("{taskId}/total")]
        public async Task<IActionResult> GetTotalTimeTrackings([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.GetTotalTimeAsync(requesterId, taskId);
            if(result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Total tracking not found",
                    Trackings = result
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Total trackings retrieved successfully",
                TotalTrackings = result
            });
        }
        [HttpPost("{taskId}")]
        public async Task<IActionResult> StartTracking([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.StartTrackingAsync(requesterId, taskId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{timeTrackingId}")]
        public async Task<IActionResult> StopTracking([FromRoute] Guid timeTrackingId, [FromBody] StopTrackingRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.StopTrackingAsync(requesterId, timeTrackingId, request);
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
