using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.UserRequest;

namespace Mutqan.PL.Area.SuperAdmin
{
    [Area("SuperAdmin")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : ControllerBase
    {
        private readonly IManageUsersService _manageUsersService;

        public UsersController(IManageUsersService manageUsersService)
        {
            _manageUsersService = manageUsersService;
        }
        [HttpGet()]
        public async Task<IActionResult> Index() => Ok(await _manageUsersService.GetUsersAsync());
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string userId)
        {
            var result = await _manageUsersService.GetUserDetailsAsync(userId);
            if(result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "User not found"
                });
            }
            return Ok(new
            {
                Success = false,
                Message = "User retrieved successfully",
                User = result
            });
        }
        [HttpPatch("BlockUser/{userId}")]
        public async Task<IActionResult> BlockUser([FromRoute] string userId)
        {
            var result = await _manageUsersService.BlockedUserAsync(userId);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("UnBlockUser/{userId}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string userId)
        {
            var result = await _manageUsersService.UnBlockedUserAsync(userId);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequest request)
        {
            var result = await _manageUsersService.ChangeUserRoleAsync(request);
            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
