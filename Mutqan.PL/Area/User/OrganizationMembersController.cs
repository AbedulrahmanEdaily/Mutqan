using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Class;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.Models;
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
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.AddUserToOrganizationAsync(adminId,request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserFromOrganization([FromRoute] string userId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.RemoveUserFromOrganizationAsync(adminId, userId);
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
        public async Task<IActionResult> GetAllMember()
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _organizationMemberService.GetAllMemberAsync(requesterId);
            return Ok(new 
            { 
                Success = true, 
                Message = "Organization Members retrieved successfully", 
                OrganizationMembers = result 
            });
        }
        
    }
}
