using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.DTO.Response.TimeTrackingResponse
{
    public class GetTotalTimeTrackingResponse
    {
        public List<TimeTrackingSummaryResponse> TimeTrackings { get; set; }
        public double TotalDuration => TimeTrackings?.Sum(t => t.Duration ?? 0) ?? 0;
    }
}
