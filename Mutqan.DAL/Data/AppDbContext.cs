using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Models;
using System.Security.Claims;
using ProjectTask = Mutqan.DAL.Models.ProjectTask;

namespace Mutqan.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationMember> OrganizationMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TimeTracking> TimeTrackings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserOrganizationHistory> UserOrganizationHistories { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            builder.Entity<TaskDependency>(entity =>
            {
                entity.HasKey(x => new { x.TaskId, x.DependsOnTaskId });
                entity.HasOne(x => x.Task)
                    .WithMany(t => t.Dependencies)
                    .HasForeignKey(x => x.TaskId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.DependsOn)
                    .WithMany(t => t.DependentOn)
                    .HasForeignKey(x => x.DependsOnTaskId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ProjectTask>().HasOne(t => t.AssignedTo)
                    .WithMany()
                    .HasForeignKey(t => t.AssignedToUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Attachment>().HasOne(t => t.UploadedBy)
                    .WithMany()
                    .HasForeignKey(t => t.UploadedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TaskHistory>().HasOne(t => t.ChangedBy)
                    .WithMany()
                    .HasForeignKey(t => t.ChangedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Organization>()
                .HasMany(o => o.OrganizationMember)        
                .WithOne(m => m.Organization)
                .HasForeignKey(m => m.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var entries = ChangeTracker.Entries<BaseModel>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        if (entityEntry.Property(x => x.IsDeleted).CurrentValue)
                        {
                            entityEntry.Property(x => x.DeletedBy).CurrentValue = currentUserId;
                            entityEntry.Property(x => x.DeletedAt).CurrentValue = DateTime.UtcNow;
                            continue;
                        }
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    }
                }
                var taskEntries = ChangeTracker.Entries<ProjectTask>().Where(e => e.State == EntityState.Modified);
                foreach (var entry in taskEntries)
                {
                    foreach (var prop in entry.Properties.Where(p => p.IsModified))
                    {
                        var history = new TaskHistory
                        {
                            TaskId = entry.Entity.Id,
                            ChangedByUserId = currentUserId!,
                            FieldChanged = prop.Metadata.Name,
                            OldValue = prop.OriginalValue?.ToString(),
                            NewValue = prop.CurrentValue?.ToString(),
                        };
                        await TaskHistories.AddAsync(history);
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            if (_httpContextAccessor.HttpContext != null)
            {

                var entries = ChangeTracker.Entries<BaseModel>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        if (entityEntry.Property(x => x.IsDeleted).CurrentValue)
                        {
                            entityEntry.Property(x => x.DeletedBy).CurrentValue = currentUserId;
                            entityEntry.Property(x => x.DeletedAt).CurrentValue = DateTime.UtcNow;
                            continue;
                        }
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    }
                }
                var taskEntries = ChangeTracker.Entries<ProjectTask>().Where(e => e.State == EntityState.Modified);
                foreach (var entry in taskEntries)
                {
                    foreach (var prop in entry.Properties.Where(p => p.IsModified))
                    {
                        var history = new TaskHistory
                        {
                            TaskId = entry.Entity.Id,
                            ChangedByUserId = currentUserId!,
                            FieldChanged = prop.Metadata.Name,
                            OldValue = prop.OriginalValue?.ToString(),
                            NewValue = prop.CurrentValue?.ToString(),
                        };
                        TaskHistories.AddAsync(history);
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
