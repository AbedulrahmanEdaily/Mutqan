using Mutqan.DAL.DTO.Request.TimeTrackingRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.TimeTrackingResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface ITimeTrackingService : IScopedService
    {
        Task<BaseResponse> StartTrackingAsync(string requesterId, Guid taskId);
        Task<BaseResponse> StopTrackingAsync(string requesterId, Guid timeTrackingId, StopTrackingRequest request);
        Task<List<GetTimeTrackingResponse>> GetTaskTimeTrackingsAsync(string requesterId, Guid taskId);
        Task<GetTotalTimeTrackingResponse?> GetTotalTimeAsync(string requesterId, Guid taskId);
    }
}
