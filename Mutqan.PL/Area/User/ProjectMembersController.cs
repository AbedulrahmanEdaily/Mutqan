using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;

        public ProjectMembersController(IProjectMemberService projectMemberService)
        {
            _projectMemberService = projectMemberService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToProject([FromBody]AddProjectMemberRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectMemberService.AddUserToProjectAsync(adminId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{projectId}/member/{userId}")]
        public async Task<IActionResult> RemoveUserFromProject([FromRoute]Guid projectId, [FromRoute] string userId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectMemberService.RemoveUserFromProjectAsync(adminId, projectId, userId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{projectId}/members")]
        public async Task<IActionResult> GetAllProjectMembers([FromRoute]Guid projectId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectMemberService.GetAllProjectMembersAsync(adminId,projectId);
            return Ok(new
            {
                Success = true,
                Message = "Project Members retrieved successfully",
                ProjectMembers = result
            });
        }
        [HttpGet("GetProjectMemberById/{projectMemberId}")]
        public async Task<IActionResult> GetProjectMemberbyId([FromRoute]Guid projectMemberId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectMemberService.GetProjectMemberByIdAsync(adminId, projectMemberId);
            if (result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Project Member not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Project Member retrieved successfully",
                ProjectMember = result
            });
        }
    }
}
