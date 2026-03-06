using Mutqan.DAL.DTO.Request.SprintRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.SprintResponse;


namespace Mutqan.BLL.Services.Interface
{
    public interface ISprintService:IScopedService
    {
        Task<List<SprintResponse>> GetAllSprintsAsync(string adminId, Guid projectId);
        Task<SprintDetailsResponse?> GetSprintDetailsAsync(string adminId, Guid sprintId);
        Task<BaseResponse> CreateSprintAsync(string adminId, CreateSprintRequest request);
        Task<BaseResponse> UpdateSprintAsync(string adminId, Guid sprintId, UpdateSprintRequest request);
        Task<BaseResponse> DeleteSprintAsync(string adminId, Guid sprintId);
        Task<BaseResponse> StartSprintAsync(string adminId, Guid sprintId);
        Task<BaseResponse> CompleteSprintAsync(string adminId, Guid sprintId);
    }
}
