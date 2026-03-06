using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.SprintResponse
{
    public class SprintResponse
    {
        public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SprintStatus Status { get; set; }
    }
}
