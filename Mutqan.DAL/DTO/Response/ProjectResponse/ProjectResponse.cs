using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.ProjectResponse
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus Status { get; set; }
    }
}
