using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using System.Security.Claims;

namespace Mutqan.PL.Area.SuperAdmin
{
    [Area("SuperAdmin")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody]OrganizationRequest request)
        {
            var result = await _organizationService.CreateOrganizationAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization([FromRoute]Guid id)
        {
            var result = await _organizationService.DeleteOrganizationAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrganization([FromRoute]Guid id, [FromBody] OrganizationRequest request)
        {
            var result = await _organizationService.UpdateOrganizationAsync(id,request);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var result = await _organizationService.GetAllOrganizationsAsync();
            return Ok(new {Success = true,Message = "Organizations retrieved successfully", Organizations =  result });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizationById([FromRoute]Guid id)
        {
            var result = await _organizationService.GetOrganizationByIdAsync(id);
            if(result is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Organization not found"
                });
            }
            return Ok(new 
            {
                Success = true,
                Message = "Organizations retrieved successfully",
                Organization =  result 
            });
        }
    }
}
