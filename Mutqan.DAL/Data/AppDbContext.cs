using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Models;
using ProjectTask = Mutqan.DAL.Models.ProjectTask;

namespace Mutqan.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Organization> Organizations { get; set; }
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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

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
        }
    }
}
