using System;
using System.Collections.Generic;
using System.Text;

namespace Mutqan.DAL.Models
{
    public enum NotificationType
    {
        TaskAssigned,
        CommentAdded,
        SprintStarted,
        SprintCompleted,
        TaskStatusChanged
    }
    public class Notification 
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
    }
}
