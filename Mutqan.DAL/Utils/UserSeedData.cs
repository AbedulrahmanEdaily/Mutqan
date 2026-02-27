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

        public UserSeedData(AppDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async System.Threading.Tasks.Task DataSeed()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
           
            if (!await _roleManager.Roles.AnyAsync())
            {
                string[] Rols = ["SuperAdmin", "User"];
                foreach (var role in Rols)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Abd",
                    Email = "abd@gmail.com",
                    FullName = "Abdulrahman Edaily",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    UserName = "tariq",
                    Email = "tariq@gmail.com",
                    FullName = "Tariq Shreem",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
                {
                    UserName = "Khaled",
                    Email = "khaled@gmail.com",
                    FullName = "Khaled Ahmed",
                    EmailConfirmed = true
                };
            
                await _userManager.CreateAsync(user1, "Abd@12345");
                await _userManager.CreateAsync(user2, "Tariq@12345");
                await _userManager.CreateAsync(user3, "Khaled@12345");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "User");
                await _userManager.AddToRoleAsync(user3, "User");
                
            }
            if (!await _context.Organizations.AnyAsync())
            {
                await _context.Organizations.AddAsync(new Organization
                {
                    Id = Guid.NewGuid(),
                    Name = "Mutqan",
                    Description = "This is Mutqan",
                    CreatedBy = "Seeder"
                });
                await _context.SaveChangesAsync();
            }
            if (!await _context.OrganizationMembers.AnyAsync())
            {
                var org = await _context.Organizations
                    .FirstAsync(o => o.Name == "Mutqan");
                var user = await _userManager.FindByNameAsync("tariq");
                await _context.OrganizationMembers.AddAsync(new OrganizationMember
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = org.Id,
                    UserId = user.Id,
                    Role = OrganizationRole.Admin
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
