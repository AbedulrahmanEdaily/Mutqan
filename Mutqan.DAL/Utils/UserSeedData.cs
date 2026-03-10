using Mutqan.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mutqan.DAL.Data;

namespace Mutqan.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserSeedData(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async System.Threading.Tasks.Task DataSeed()
        {
            // ── Migrations ──────────────────────────────────────────────────────
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                await _context.Database.MigrateAsync();

            // ── Roles ───────────────────────────────────────────────────────────
            if (!await _roleManager.Roles.AnyAsync())
            {
                foreach (var role in new[] { "SuperAdmin", "User" })
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // ── Users ───────────────────────────────────────────────────────────
            if (!await _userManager.Users.AnyAsync())
            {
                // SuperAdmin
                var superAdmin = new ApplicationUser
                {
                    UserName = "Abd",
                    Email = "abd@gmail.com",
                    FullName = "Abdulrahman Edaily",
                    EmailConfirmed = true
                };

                // OrgAdmin
                var orgAdmin = new ApplicationUser
                {
                    UserName = "tariq",
                    Email = "tariq@gmail.com",
                    FullName = "Tariq Shreem",
                    EmailConfirmed = true
                };

                // Developer
                var developer = new ApplicationUser
                {
                    UserName = "Khaled",
                    Email = "khaled@gmail.com",
                    FullName = "Khaled Ahmed",
                    EmailConfirmed = true
                };

                // Developer 2
                var developer2 = new ApplicationUser
                {
                    UserName = "Sara",
                    Email = "sara@gmail.com",
                    FullName = "Sara Ali",
                    EmailConfirmed = true
                };

                // Client
                var client = new ApplicationUser
                {
                    UserName = "Omar",
                    Email = "omar@gmail.com",
                    FullName = "Omar Hassan",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(superAdmin, "Abd@12345");
                await _userManager.CreateAsync(orgAdmin, "Tariq@12345");
                await _userManager.CreateAsync(developer, "Khaled@12345");
                await _userManager.CreateAsync(developer2, "Sara@12345");
                await _userManager.CreateAsync(client, "Omar@12345");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(orgAdmin, "User");
                await _userManager.AddToRoleAsync(developer, "User");
                await _userManager.AddToRoleAsync(developer2, "User");
                await _userManager.AddToRoleAsync(client, "User");
            }

            // ── Organization ────────────────────────────────────────────────────
            if (!await _context.Organizations.AnyAsync())
            {
                var org = new Organization
                {
                    Id = Guid.NewGuid(),
                    Name = "Mutqan",
                    Description = "شركة متقن لتطوير البرمجيات",
                    CreatedBy = "Seeder"
                };
                await _context.Organizations.AddAsync(org);
                await _context.SaveChangesAsync();
            }

            // ── Organization Members ────────────────────────────────────────────
            if (!await _context.OrganizationMembers.AnyAsync())
            {
                var org = await _context.Organizations.FirstAsync(o => o.Name == "Mutqan");
                var tariq = await _userManager.FindByNameAsync("tariq");
                var khaled = await _userManager.FindByNameAsync("Khaled");
                var sara = await _userManager.FindByNameAsync("Sara");
                var omar = await _userManager.FindByNameAsync("Omar");

                var members = new List<OrganizationMember>
            {
                new() { Id = Guid.NewGuid(), OrganizationId = org.Id, UserId = tariq.Id, Role = OrganizationRole.Admin },
                new() { Id = Guid.NewGuid(), OrganizationId = org.Id, UserId = khaled.Id, Role = OrganizationRole.Member },
                new() { Id = Guid.NewGuid(), OrganizationId = org.Id, UserId = sara.Id, Role = OrganizationRole.Member },
                new() { Id = Guid.NewGuid(), OrganizationId = org.Id, UserId = omar.Id, Role = OrganizationRole.Member },
            };

                await _context.OrganizationMembers.AddRangeAsync(members);
                await _context.SaveChangesAsync();
            }

            // ── Project ─────────────────────────────────────────────────────────
            if (!await _context.Projects.AnyAsync())
            {
                var org = await _context.Organizations.FirstAsync(o => o.Name == "Mutqan");

                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Mutqan Platform",
                    Description = "منصة إدارة المشاريع متعددة المستأجرين",
                    OrganizationId = org.Id,
                    Status = ProjectStatus.Active,
                    CreatedBy = "Seeder"
                };

                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
            }

            // ── Project Members ─────────────────────────────────────────────────
            if (!await _context.ProjectMembers.AnyAsync())
            {
                var project = await _context.Projects.FirstAsync(p => p.Name == "Mutqan Platform");
                var tariq = await _userManager.FindByNameAsync("tariq");
                var khaled = await _userManager.FindByNameAsync("Khaled");
                var sara = await _userManager.FindByNameAsync("Sara");
                var omar = await _userManager.FindByNameAsync("Omar");

                var projectMembers = new List<ProjectMember>
            {
                new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = tariq.Id, Role = ProjectRole.ProjectManager },
                new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = khaled.Id, Role = ProjectRole.Developer },
                new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = sara.Id, Role = ProjectRole.Developer },
                new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = omar.Id, Role = ProjectRole.Client },
            };

                await _context.ProjectMembers.AddRangeAsync(projectMembers);
                await _context.SaveChangesAsync();
            }

            // ── Sprint ──────────────────────────────────────────────────────────
            if (!await _context.Sprints.AnyAsync())
            {
                var project = await _context.Projects.FirstAsync(p => p.Name == "Mutqan Platform");

                var sprint = new Sprint
                {
                    Id = Guid.NewGuid(),
                    Name = "Sprint 1",
                    ProjectId = project.Id,
                    Status = SprintStatus.Planning,
                    EstimatedStartDate = DateTime.UtcNow,
                    EstimatedEndDate = DateTime.UtcNow.AddDays(14),
                    CreatedBy = "Seeder"
                };

                await _context.Sprints.AddAsync(sprint);
                await _context.SaveChangesAsync();
            }

            // ── Tasks ───────────────────────────────────────────────────────────
            if (!await _context.Tasks.AnyAsync())
            {
                var project = await _context.Projects.FirstAsync(p => p.Name == "Mutqan Platform");
                var khaled = await _userManager.FindByNameAsync("Khaled");
                var sara = await _userManager.FindByNameAsync("Sara");

                var tasks = new List<ProjectTask>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "تصميم قاعدة البيانات",
                    Description = "تصميم جداول قاعدة البيانات للمشروع",
                    ProjectId = project.Id,
                    AssignedToUserId = khaled.Id,
                    Status = DAL.Models.TaskStatus.Backlog,
                    Priority = TaskPriority.High,
                    EstimatedStartDate = DateTime.UtcNow,
                    EstimatedEndDate = DateTime.UtcNow.AddDays(3),
                    CreatedBy = "Seeder"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "تطوير API المصادقة",
                    Description = "بناء نظام JWT وGoogle OAuth",
                    ProjectId = project.Id,
                    AssignedToUserId = khaled.Id,
                    Status = DAL.Models.TaskStatus.Backlog,
                    Priority = TaskPriority.Critical,
                    EstimatedStartDate = DateTime.UtcNow,
                    EstimatedEndDate = DateTime.UtcNow.AddDays(5),
                    CreatedBy = "Seeder"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "تطوير واجهة المستخدم",
                    Description = "بناء الواجهة الأمامية بـ React",
                    ProjectId = project.Id,
                    AssignedToUserId = sara.Id,
                    Status = DAL.Models.TaskStatus.Backlog,
                    Priority = TaskPriority.Medium,
                    EstimatedStartDate = DateTime.UtcNow.AddDays(3),
                    EstimatedEndDate = DateTime.UtcNow.AddDays(10),
                    CreatedBy = "Seeder"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "اختبار النظام",
                    Description = "كتابة اختبارات الوحدة والتكامل",
                    ProjectId = project.Id,
                    AssignedToUserId = sara.Id,
                    Status = DAL.Models.TaskStatus.Backlog,
                    Priority = TaskPriority.Low,
                    EstimatedStartDate = DateTime.UtcNow.AddDays(10),
                    EstimatedEndDate = DateTime.UtcNow.AddDays(14),
                    CreatedBy = "Seeder"
                },
            };

                await _context.Tasks.AddRangeAsync(tasks);
                await _context.SaveChangesAsync();
            }
        }
    }
}

