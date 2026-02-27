using Mapster;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;


namespace Mutqan.BLL.Services.Class
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }
        public async Task<BaseResponse> CreateOrganizationAsync(OrganizationRequest request)
        {
            if(request is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Organization created faild"
                };
            }
            var result = request.Adapt<Organization>();
            await _organizationRepository.CreateAsync(result);
            return new BaseResponse
            {
                Success = true,
                Message = "Organization created successfully"
            };
        }
        public async Task<List<OrganizationResponse>> GetAllOrganizationsAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            return organizations.Adapt<List<OrganizationResponse>>();
        }
        public async Task<OrganizationResponse?> GetOrganizationByIdAsync(Guid id)
        {
            var organization = await _organizationRepository.FindByIdAsync(id);
            return organization.Adapt<OrganizationResponse>();
        }
        public async Task<BaseResponse> UpdateOrganizationAsync(Guid id,OrganizationRequest request)
        {
            var organization = await _organizationRepository.FindByIdAsync(id);
            if(organization is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }
            request.Adapt(organization);
            await _organizationRepository.UpdateAsync(organization);
            return new BaseResponse
            {
                Success = true,
                Message = "Organization updated successfully"
            };
        }
        public async Task<BaseResponse> DeleteOrganizationAsync(Guid id)
        {
            var organization = await _organizationRepository.FindByIdAsync(id);
            if(organization is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }
            await _organizationRepository.DeleteAsync(organization);
            return new BaseResponse
            {
                Success = true,
                Message = "Organization deleted successfully"
            };
        }
    }
}
