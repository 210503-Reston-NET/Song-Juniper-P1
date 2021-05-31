using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoreModels;

namespace StoreWebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                    var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();

                    SeedData(userManager, roleManager);
                }
                catch
                {

                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SeedData(UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                UserRole adminRole = new UserRole();
                adminRole.Id = Guid.NewGuid();
                adminRole.Name = "Admin";
                adminRole.NormalizedName = "ADMIN";
                adminRole.ConcurrencyStamp = Guid.NewGuid().ToString();
                IdentityResult roleResult = roleManager.CreateAsync(adminRole).Result;
            }
            if (userManager.FindByNameAsync("auryn@isadmin.com").Result == null)
            {
                User appUser = new User();
                appUser.Id = Guid.NewGuid();
                appUser.Email = "auryn@isadmin.com";
                appUser.NormalizedEmail = "AURYN@ISADMIN.COM";
                appUser.EmailConfirmed = true;
                appUser.Name = "Auryn The Admin";
                appUser.UserName = "auryn@isadmin.com";
                appUser.NormalizedUserName = "AURYN@ISADMIN.COM";

                IdentityResult result = userManager.CreateAsync
                (appUser, "Testest1!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(appUser, "Admin").Wait();
                }
            }
        }
    }
}
