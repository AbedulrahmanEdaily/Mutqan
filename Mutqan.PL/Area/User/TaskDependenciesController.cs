using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TaskDependenciesController : ControllerBase
    {
        private readonly ITaskDependencyService _taskDependencyService;

        public TaskDependenciesController(ITaskDependencyService taskDependencyService)
        {
            _taskDependencyService = taskDependencyService;
        }
        [HttpPost("{taskId}/DependsOn/{dependsOnId}")]
        public async Task<IActionResult> AddDependency([FromRoute]Guid taskId, [FromRoute] Guid dependsOnId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _taskDependencyService.AddDependencyAsync(requesterId, taskId, dependsOnId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{taskId}/RemoveDependsOn/{dependsOnId}")]
        public async Task<IActionResult> RemoveDependency([FromRoute]Guid taskId, [FromRoute] Guid dependsOnId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _taskDependencyService.RemoveDependencyAsync(requesterId, taskId, dependsOnId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetDependencies([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _taskDependencyService.GetDependenciesAsync(requesterId, taskId);
            return Ok(new
            {
                Success = true,
                Message = "Dependencies retrieved successfully",
                Dependencies = result
            });
        }
    }
}
