using Mutqan.DAL.Models;

using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.ProjectResponse
{
    public class ProjectMemberResponse
    {
        public Guid ProjectMemberId { get; set; }
        public string UserId { get; set; }
        public string FullName { set; get; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
