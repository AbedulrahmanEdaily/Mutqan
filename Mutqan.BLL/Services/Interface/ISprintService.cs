using Mutqan.DAL.DTO.Request.SprintRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.SprintResponse;

namespace Mutqan.BLL.Services.Interface
{
    public interface ISprintService:IScopedService
    {
        Task<List<SprintResponse>> GetAllSprintsAsync(string requesterId, Guid projectId);
        Task<SprintDetailsResponse?> GetSprintDetailsAsync(string requesterId, Guid sprintId);
        Task<BaseResponse> CreateSprintAsync(string requesterId, CreateSprintRequest request);
        Task<BaseResponse> UpdateSprintAsync(string requesterId, Guid sprintId, UpdateSprintRequest request);
        Task<BaseResponse> DeleteSprintAsync(string requesterId, Guid sprintId);
        Task<BaseResponse> StartSprintAsync(string requesterId, Guid sprintId);
        Task<BaseResponse> CompleteSprintAsync(string requesterId, Guid sprintId);
    }
}
