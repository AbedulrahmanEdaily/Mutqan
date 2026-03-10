using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.TaskRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class TasksController : ControllerBase
    {
        private readonly IProjectTaskService _projectTaskService;

        public TasksController(IProjectTaskService projectTaskService)
        {
            _projectTaskService = projectTaskService;
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetAllTasks([FromRoute] Guid projectId,[FromQuery] int limit = 3,[FromQuery] int page = 1)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.GetAllTasksAsync(requesterId, projectId, limit, page);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("TaskDetails/{taskId}")]
        public async Task<IActionResult> GetTaskDetailsById([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.GetTaskDetailsAsync(requesterId, taskId);
            if (result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Task not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Task retrieved successfully",
                Task = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.CreateTaskAsync(requesterId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody]UpdateTaskRequest request, [FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.UpdateTaskAsync(requesterId, taskId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.DeleteTaskAsync(requesterId, taskId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("AddTaskToSprint")]
        public async Task<IActionResult> AddTaskToSprint([FromBody]AddTaskToSprintRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.AddTaskToSprintAsync(requesterId,request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("RemoveTaskFromSprint/{taskId}")]
        public async Task<IActionResult> RemoveTaskFromSprint([FromRoute] Guid taskId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.RemoveTaskFromSprintAsync(requesterId, taskId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("ChangeTaskPriority/{taskId}")]
        public async Task<IActionResult> ChangeTaskPriority([FromRoute] Guid taskId, [FromBody] ChangeTaskPriorityRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.ChangeTaskPriorityAsync(requesterId, taskId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("ChangeTaskStatus/{taskId}")]
        public async Task<IActionResult> ChangeTaskStatus([FromRoute] Guid taskId, [FromBody] ChangeTaskStatusRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.ChangeTaskStatusAsync(requesterId, taskId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("AssignTaskToDeveloper/{taskId}")]
        public async Task<IActionResult> AssignTaskToDeveloper([FromRoute] Guid taskId, [FromBody] AssignTaskToDeveloperRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.AssignTaskToDeveloperAsync(requesterId, taskId, request);
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
