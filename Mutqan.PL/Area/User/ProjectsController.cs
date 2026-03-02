using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.ProjectRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.GetAllProjectAsync(adminId);
            return Ok(new
            {
                Success = true,
                Message = "Projects retrieved successfully",
                Projects = result
            });
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid projectId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.GetProjectByIdAsync(adminId, projectId);
            if (result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Project not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Project retrieved successfully",
                Project = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.CreateProjectAsync(adminId, request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch()]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.UpdateProjectAsync(adminId, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.DeleteProjectAsync(adminId, projectId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("ChangeProjectStatus")]
        public async Task<IActionResult> ChangeProjectStatus([FromBody]ChangeProjectStatusRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _projectService.ChangeProjectStatusAsync(adminId,request);
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
