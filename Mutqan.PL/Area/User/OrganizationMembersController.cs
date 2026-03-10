using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.User
{
    [Area("User")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,SuperAdmin")]
    public class OrganizationMembersController : ControllerBase
    {
        private readonly IOrganizationMemberService _organizationMemberService;

        public OrganizationMembersController(IOrganizationMemberService organizationMemberService)
        {
            _organizationMemberService = organizationMemberService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToOrganization([FromBody]OrganizationMemberRequest request)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.AddUserToOrganizationAsync(requesterId,request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserFromOrganization([FromRoute] string userId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.RemoveUserFromOrganizationAsync(requesterId, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMemberByUserId([FromRoute] string userId)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.GetMemberByUserIdAsync(userId, requesterId);
            if (result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Organization Member not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Organizations Member retrieved successfully",
                OrganizationMember = result
            });
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllMember([FromQuery] int limit = 3, [FromQuery] int page = 1)
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.GetAllMemberAsync(requesterId, page, limit);
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
