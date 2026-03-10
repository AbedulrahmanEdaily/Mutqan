using Mutqan.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mutqan.DAL.DTO.Response.NotificationResponse
{
    public class GetAllNotificationResponse
    {
        public Guid Id { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
