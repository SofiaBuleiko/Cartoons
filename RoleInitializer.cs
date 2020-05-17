using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartoonsWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CartoonsWebApp
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "Admin";
            string adminEmail = "sofiabulejko@gmail.com";
            string password = "Sofia.2000";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { Name = adminName, UserName = adminName, Email = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
