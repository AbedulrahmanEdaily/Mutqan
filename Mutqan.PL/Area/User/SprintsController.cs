using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using Mutqan.DAL.DTO.Request.SprintRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class SprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;

        public SprintsController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetAllSprints([FromRoute] Guid projectId )
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.GetAllSprintsAsync(requesterId, projectId);
            return Ok(new
            {
                Success = true,
                Message = "Sprints retrieved successfully",
                Sprints = result
            });
        }
        [HttpGet("SprintDetails/{sprintId}")]
        public async Task<IActionResult> GetSprintDetailsById([FromRoute] Guid sprintId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.GetSprintDetailsAsync(requesterId, sprintId);
            if (result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Sprint not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Sprint retrieved successfully",
                Sprint = result
            });
        }
        [HttpPost()]
        public async Task<IActionResult> CreateSprint([FromBody]CreateSprintRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.CreateSprintAsync(requesterId, request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{sprintId}")]
        public async Task<IActionResult> UpdateSprint([FromRoute] Guid sprintId, [FromBody] UpdateSprintRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.UpdateSprintAsync(requesterId, sprintId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{sprintId}/start")]
        public async Task<IActionResult> StartSprint([FromRoute] Guid sprintId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.StartSprintAsync(requesterId, sprintId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("{sprintId}/complete")]
        public async Task<IActionResult> CompleteSprint([FromRoute] Guid sprintId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.CompleteSprintAsync(requesterId, sprintId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{sprintId}")]
        public async Task<IActionResult> DeleteSprint([FromRoute] Guid sprintId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _sprintService.DeleteSprintAsync(requesterId, sprintId);
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
