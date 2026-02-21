using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.Models;
using Task = Mutqan.DAL.Models.Task;

namespace Mutqan.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            base.OnModelCreating(builder);

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
            builder.Entity<Task>().HasOne(t => t.AssignedTo)
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
