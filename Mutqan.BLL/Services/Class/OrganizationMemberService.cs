using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.OrganizationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.OrganizationResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Class;
using Mutqan.DAL.Repository.Interface;

namespace Mutqan.BLL.Services.Class
{
    public class OrganizationMemberService : IOrganizationMemberService
    {
        private readonly IOrganizationMemberRepository _organizationMemberRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserOrgainzatinHistoryRepository _userOrgainzatinHistoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationMemberService(IOrganizationMemberRepository organizationMemberRepository,
             IOrganizationRepository organizationRepository
            ,IUserOrgainzatinHistoryRepository userOrgainzatinHistoryRepository
            ,UserManager<ApplicationUser> userManager
            )
        {
            _organizationMemberRepository = organizationMemberRepository;
            _organizationRepository = organizationRepository;
            _userOrgainzatinHistoryRepository = userOrgainzatinHistoryRepository;
            _userManager = userManager;
        }
        public async Task<BaseResponse> AddUserToOrganizationAsync(string requesterId,OrganizationMemberRequest request)
        {
            var adminUser = await _userManager.FindByIdAsync(requesterId);
            if(!await _organizationMemberRepository.IsOrganizationAdminAsync(requesterId, request.OrganizationId) && !await _userManager.IsInRoleAsync(adminUser,"SuperAdmin"))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User doesn't exist"
                };
            }
            var organization = await _organizationRepository.FindByIdAsync(request.OrganizationId);
            if(organization is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Organization doesn't exist"
                };
            }
            if(await _organizationMemberRepository.IsUserInOrganizationAsync(user.Id))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User has already in organization"
                };
            }
            var member = new OrganizationMember
            {
                OrganizationId = request.OrganizationId,
                UserId = request.UserId,
                Role = request.Role
            };
            await _organizationMemberRepository.CreateAsync(member);
            var UserOrganizationHistory = new UserOrganizationHistory
            {
                OrganizationMemberId = member.Id,
                OrganizationId = organization.Id,
            };
            await _userOrgainzatinHistoryRepository.CreateAsync(UserOrganizationHistory);
            return new BaseResponse
            {
                Success = true,
                Message = "User added successfully"
            };
        }
        public async Task<OrganizationMemberResponse?> GetMemberByUserIdAsync(string userId, string userRequested)
        {
            var requesterMember = await _organizationMemberRepository.GetByUserIdAsync(userRequested);
            var member = await _organizationMemberRepository.GetByUserIdAsync(userId);
            if(member is null || requesterMember is null)
            {
                return null;
            }
            if(requesterMember.OrganizationId != member.OrganizationId)
            {
                return null;
            }
            return member.Adapt<OrganizationMemberResponse>();
        }
        public async Task<List<OrganizationMemberResponse>> GetAllMemberAsync(string requesterId)
        {
            var requesterMember = await _organizationMemberRepository.GetByUserIdAsync(requesterId);

            if (requesterMember is null)
                return new List<OrganizationMemberResponse>();

            var members = await _organizationMemberRepository
                .GetByOrganizationIdAsync(requesterMember.OrganizationId);
            return members.Adapt<List<OrganizationMemberResponse>>();
        }
        public async Task<BaseResponse> RemoveUserFromOrganizationAsync(string requesterId,string userId)
        {
            var member = await _organizationMemberRepository.GetByUserIdAsync(userId);

            if (member is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User is not in any organization"
                };
            }
            var adminUser = await _userManager.FindByIdAsync(requesterId);
            var isOrganizationAdmin = await _organizationMemberRepository.IsOrganizationAdminAsync(requesterId, member.OrganizationId);
            if (! isOrganizationAdmin && !await _userManager.IsInRoleAsync(adminUser, "SuperAdmin"))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User not allowed"
                };
            }
            if (requesterId == userId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't remove yourself from organization"
                };
            } 
            if (isOrganizationAdmin && member.Role == OrganizationRole.Admin)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't remove an admin"
                };
            }
            await _organizationMemberRepository.DeleteAsync(member);
            var history = await _userOrgainzatinHistoryRepository
                .GetActiveByOrganizationMemberIdAsync(member.Id);

            if (history is not null)
            {
                history.LeftAt = DateTime.UtcNow;
                await _userOrgainzatinHistoryRepository.UpdateAsync(history);
            }

            return new BaseResponse
            {
                Success = true,
                Message = "User removed from organization successfully"
            };
        }
    }
}
