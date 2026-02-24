using Mutqan.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutqan.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserSeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async System.Threading.Tasks.Task DataSeed()
        {
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
        }
    }
}
