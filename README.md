# Mutqan (متقن) 🚀

A multi-tenant Scrum project management platform built for software teams.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Roles & Permissions](#roles--permissions)
- [Modules](#modules)
- [API Endpoints](#api-endpoints)
- [Getting Started](#getting-started)
- [Configuration](#configuration)

---

## Overview

Mutqan is a robust, multi-tenant Scrum-based project management system that enables organizations to manage projects, sprints, tasks, and team collaboration in real time. It supports multiple organizations, each with their own projects, members, and workflows.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT + Refresh Tokens + Google OAuth |
| Real-Time | SignalR |
| File Storage | Cloudinary |
| Object Mapping | Mapster |
| Architecture | N-Tier (DAL / BLL / PL) |

---

## Architecture

```
Mutqan.DAL   → Models, DbContext, Repositories, Migrations
Mutqan.BLL   → Services, DTOs, Business Logic
Mutqan.PL    → Controllers, Hubs, Program.cs
```

---

## Roles & Permissions

```
System Level
└── SuperAdmin         → Full platform control

Organization Level
└── OrganizationAdmin  → Controls their organization

Project Level
├── ProjectManager     → Controls their projects
├── Developer          → Works on assigned tasks
└── Client             → View only
```

---

## Modules

### 🔐 Authentication
- Register with email confirmation
- Login with JWT Access Token (15 min) + Refresh Token
- Forget Password with 8-digit code
- Reset Password
- Google OAuth
- Refresh Token rotation with `IsRevoked` audit trail

---

### 🏢 Organization Management
- SuperAdmin creates and manages organizations
- OrganizationAdmin manages members within their organization
- Member history tracked via `UserOrganizationHistory`

---

### 👥 Organization Members
- Add / Remove members
- Roles: `Admin`, `Member`
- Only SuperAdmin can assign Admin role
- Self-remove and Admin-remove protection

---

### 📁 Project Management
- OrganizationAdmin creates / updates / deletes projects
- Project statuses: `Active`, `OnHold`, `Completed`, `Archived`
- ProjectManager and Members can view their projects

---

### 👤 Project Members
- Roles: `ProjectManager`, `Developer`, `Client`
- One ProjectManager per project
- Members must belong to the same organization

---

### 🏃 Sprint Management
- ProjectManager creates and manages sprints
- Sprint statuses: `Planning`, `Active`, `Completed`
- Only one active sprint per project at a time
- Uncompleted tasks automatically moved to Backlog on sprint complete/delete

---

### ✅ Task Management
- Tasks always created as `Backlog`
- Task status flow:
```
Backlog → Todo → InProgress → Review → Done
```
- Only ProjectManager can move tasks to/from sprint
- Developer can change: `Todo → InProgress → Review`
- ProjectManager can change to `Done`
- `ActualStartDate` set on `InProgress`, `ActualEndDate` set on `Done`
- Task editing and deletion only allowed in `Backlog`

---

### 🔗 Task Dependencies
- Tasks can depend on other tasks
- Circular dependency detection
- Duplicate dependency prevention
- Blocking: task cannot be added to sprint if dependencies are not `Done`

---

### 💬 Comments
- All project members can comment on tasks
- Comment owner and ProjectManager can delete
- Only comment owner can edit
- `IsEdited` flag tracked

---

### 📎 Attachments
- ProjectManager can upload on any task
- Developer can upload only on their assigned task
- Files stored on Cloudinary
- Only file owner and ProjectManager can delete

---

### ⏱️ Time Tracking
- Only assigned Developer can track time
- Task must be `InProgress` to start tracking
- One active session at a time per developer
- `DurationInMinutes` calculated automatically on stop
- Notes added on stop
- ProjectManager can view all trackings

---

### 📜 Task History
- Automatically recorded on every task field change
- Tracked via `DbContext.SaveChangesAsync` override
- Audit fields excluded from history

---

### 🔔 Notifications
- Real-time via SignalR
- Persisted in database for offline users
- Triggered on:
  - Task assigned to developer
  - Task status changed
  - Comment added
  - Sprint started
  - Sprint completed
- `IsRead` tracking with mark-as-read support

---

## API Endpoints

### Authentication
```
POST   /api/Auth/register
POST   /api/Auth/confirm-email
POST   /api/Auth/login
POST   /api/Auth/forget-password
POST   /api/Auth/reset-password
POST   /api/Auth/refresh-token
GET    /api/Auth/login-google
GET    /api/Auth/signin-google
```

### Organizations
```
GET    /api/User/Organizations
GET    /api/User/Organizations/{id}
POST   /api/User/Organizations
PUT    /api/User/Organizations/{id}
DELETE /api/User/Organizations/{id}
```

### Organization Members
```
GET    /api/User/OrganizationMembers
GET    /api/User/OrganizationMembers/{userId}
POST   /api/User/OrganizationMembers
DELETE /api/User/OrganizationMembers/{userId}
```

### Projects
```
GET    /api/User/Projects
GET    /api/User/Projects/{id}
POST   /api/User/Projects
PUT    /api/User/Projects/{id}
DELETE /api/User/Projects/{id}
PATCH  /api/User/Projects/{id}/status
```

### Project Members
```
GET    /api/User/ProjectMembers/{projectId}
GET    /api/User/ProjectMembers/{memberId}
POST   /api/User/ProjectMembers
DELETE /api/User/ProjectMembers
```

### Sprints
```
GET    /api/User/Sprints/{projectId}
GET    /api/User/Sprints/{sprintId}
POST   /api/User/Sprints
PATCH  /api/User/Sprints/{sprintId}
PATCH  /api/User/Sprints/{sprintId}/start
PATCH  /api/User/Sprints/{sprintId}/complete
DELETE /api/User/Sprints/{sprintId}
```

### Tasks
```
GET    /api/User/Tasks/{projectId}
GET    /api/User/Tasks/TaskDetails/{taskId}
POST   /api/User/Tasks
PATCH  /api/User/Tasks/{taskId}
DELETE /api/User/Tasks/{taskId}
PATCH  /api/User/Tasks/ChangeTaskStatus/{taskId}
PATCH  /api/User/Tasks/AssignTaskToDeveloper/{taskId}
PATCH  /api/User/Tasks/AddTaskToSprint
PATCH  /api/User/Tasks/RemoveTaskFromSprint/{taskId}
PATCH  /api/User/Tasks/ChangeTaskPriority/{taskId}
```

### Task Dependencies
```
POST   /api/User/TaskDependencies/{taskId}/DependsOn/{dependsOnId}
DELETE /api/User/TaskDependencies/{taskId}/RemoveDependsOn/{dependsOnId}
GET    /api/User/TaskDependencies/{taskId}
```

### Comments
```
GET    /api/User/Comments/{taskId}
POST   /api/User/Comments
PATCH  /api/User/Comments/{commentId}
DELETE /api/User/Comments/{commentId}
```

### Attachments
```
GET    /api/User/Attachments/{taskId}
POST   /api/User/Attachments/{taskId}
DELETE /api/User/Attachments/{attachmentId}
```

### Time Tracking
```
GET    /api/User/TimeTrackings/{taskId}
GET    /api/User/TimeTrackings/{taskId}/total
POST   /api/User/TimeTrackings/{taskId}
PATCH  /api/User/TimeTrackings/{timeTrackingId}
```

### Notifications
```
GET    /api/User/Notifications
PATCH  /api/User/Notifications/{notificationId}
DELETE /api/User/Notifications/{notificationId}
```

### SignalR Hub
```
WS /hubs/notifications
```

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Cloudinary Account

### Installation

```bash
git clone https://github.com/AbedulrahmanEdaily/Mutqan
cd Mutqan
dotnet restore
```

### Database Setup

```bash
dotnet ef database update --project Mutqan.DAL --startup-project Mutqan.PL
```

### Run

```bash
dotnet run --project Mutqan.PL
```

---

## Configuration

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=Mutqan;..."
  },
  "Jwt": {
    "Key": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your_client_id",
      "ClientSecret": "your_client_secret"
    }
  },
  "Cloudinary": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  }
}
```

---

## Seeding

On first run, the system automatically seeds:
- Roles: `SuperAdmin`, `User`
- SuperAdmin user: `admin@mutqan.com` / `Admin@123456`

---

*Built with ❤️ by Abdulrahman*
