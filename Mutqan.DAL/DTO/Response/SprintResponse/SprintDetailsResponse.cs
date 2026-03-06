using Mutqan.DAL.DTO.Response.TaskResponse;
using Mutqan.DAL.Models;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.SprintResponse
{
    public class SprintDetailsResponse
    {
        public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        public string? Goal { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SprintStatus Status { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        //public List<ProjectTaskResponse> Tasks { get; set; }
    }
}
