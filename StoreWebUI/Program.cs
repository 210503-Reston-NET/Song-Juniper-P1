using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoreModels;
using Serilog;

namespace StoreWebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("../logs/logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args).UseSerilog().Build();
                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                        var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();

                        SeedData(userManager, roleManager);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Creating User Manager and Role Manager failed");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
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
                UserRole adminRole = new();
                adminRole.Id = Guid.NewGuid();
                adminRole.Name = "Admin";
                adminRole.NormalizedName = "ADMIN";
                adminRole.ConcurrencyStamp = Guid.NewGuid().ToString();
                _ = roleManager.CreateAsync(adminRole).Result;
            }
            if (userManager.FindByNameAsync("auryn@isadmin.com").Result == null)
            {
                User appUser = new();
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