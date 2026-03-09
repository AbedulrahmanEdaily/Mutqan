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
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskAttachments([FromRoute] Guid taskId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _attachmentService.GetTaskAttachmentsAsync(adminId, taskId);
            return Ok(new
            {
                Success = true,
                Message = "Attachments retrieved successfully",
                Attachments = result
            });
        }
        [HttpPost("{taskId}")]
        public async Task<IActionResult> AddAttachment([FromRoute]Guid taskId, IFormFile file)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _attachmentService.AddAttachmentAsync(adminId, taskId, file);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment([FromRoute]Guid attachmentId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _attachmentService.DeleteAttachmentAsync(adminId, attachmentId);
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
