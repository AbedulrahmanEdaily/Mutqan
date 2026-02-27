using Mutqan.DAL.Models;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.OrganizationResponse
{
    public class OrganizationMemberResponse
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrganizationRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
