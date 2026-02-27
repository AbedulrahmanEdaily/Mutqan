using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface IOrganizationService:IScopedService
    {
        Task<BaseResponse> CreateOrganizationAsync(OrganizationRequest request);
        Task<BaseResponse> DeleteOrganizationAsync(Guid id);
        Task<BaseResponse> UpdateOrganizationAsync(Guid id, OrganizationRequest request);
        Task<List<OrganizationResponse>> GetAllOrganizationsAsync();
        Task<OrganizationResponse?> GetOrganizationByIdAsync(Guid id);
    }
}
