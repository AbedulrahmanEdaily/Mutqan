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
        public async Task<IActionResult> GetAllTasks([FromRoute] Guid projectId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.GetAllTasksAsync(adminId, projectId);
            return Ok(new
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Tasks = result
            });
        }
        [HttpGet("TaskDetails/{taskId}")]
        public async Task<IActionResult> GetTaskDetailsById([FromRoute] Guid taskId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.GetTaskDetailsAsync(adminId, taskId);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.CreateTaskAsync(adminId, request);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.UpdateTaskAsync(adminId, taskId, request);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.DeleteTaskAsync(adminId, taskId);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.AddTaskToSprintAsync(adminId,request);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.RemoveTaskFromSprintAsync(adminId, taskId);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.ChangeTaskPriorityAsync(adminId, taskId, request);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.ChangeTaskStatusAsync(adminId, taskId, request);
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectTaskService.AssignTaskToDeveloperAsync(adminId, taskId, request);
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
