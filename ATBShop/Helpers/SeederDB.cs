using ATBShop.Constants;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATBShop.Helpers
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Migrate DB Databases");
                    var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                    context.Database.Migrate();

                    logger.LogInformation("Seeding Web Databases");
                    InitRolesAndUsers(scope);

                }
                catch (Exception ex)
                {
                    logger.LogError("Seeder working error " + ex.Message);
                }

            }
        }

        private static void InitRolesAndUsers(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!roleManager.Roles.Any())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleManager<AppRole>>>();

                var result = roleManager.CreateAsync(new AppRole
                {
                    Name = Roles.Admin
                }).Result;
                if (result.Succeeded)
                    logger.LogWarning("Create role " + Roles.Admin);
                else
                    logger.LogError("Faild create role " + Roles.Admin);
                result = roleManager.CreateAsync(new AppRole
                {
                    Name = Roles.User
                }).Result;
                if (result.Succeeded)
                    logger.LogInformation("Create role " + Roles.User);
                else
                    logger.LogError("Faild create role " + Roles.Admin);
            }
            if (!userManager.Users.Any())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserManager<AppUser>>>();

                string email = "admin@gmail.com";
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Name",
                    SecondName = "Surname",
                    PhoneNumber = "+12123123123",
                    Photo = "1.jpg"
                };
                var result = userManager.CreateAsync(user, "zxc_VBN123").Result;
                if (result.Succeeded)
                {
                    logger.LogInformation("Create user " + user.UserName);
                    result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                }
                else
                {
                    logger.LogError("Faild create user " + user.UserName);
                }
            }
        }
    }
}
