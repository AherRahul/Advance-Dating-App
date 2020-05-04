using System.Collections.Generic;
using System.Linq;
using Dating.API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace Dating.API.Data
{
    public class Seed
    {
        public static void SeedUser(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                // Create Some Role
                var roles = new List<Role>
                {
                    new Role { Name = "Member" },
                    new Role { Name = "Admin" },
                    new Role { Name = "Moderator" },
                    new Role { Name = "VIP" }
                };

                foreach (var role in roles) {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                   userManager.CreateAsync(user, "password").Wait();
                   userManager.AddToRoleAsync(user, "Member");
                }

                // Create Admin user
                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;

                    userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
                }
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}