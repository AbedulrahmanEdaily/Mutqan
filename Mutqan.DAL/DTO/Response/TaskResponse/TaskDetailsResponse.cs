using Mutqan.DAL.DTO.Response.CommentResponse;
using Mutqan.DAL.DTO.Response.FileResponse;
using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.TaskResponse
{
    public class TaskDetailsResponse
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid? SprintId { get; set; }
        public string? SprintName { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedToFullName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskPriority Priority { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Models.TaskStatus Status { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
