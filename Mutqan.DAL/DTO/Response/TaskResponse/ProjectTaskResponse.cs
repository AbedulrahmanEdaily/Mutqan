using Mutqan.DAL.DTO.Response.CommentResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.TaskResponse
{
    public class ProjectTaskResponse
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskPriority Priority { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Models.TaskStatus Status { get; set; }
        public string? AssignedToFullName { get; set; }
        public Guid? SprintId { get; set; }
        public string? SprintName { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
    }
}
