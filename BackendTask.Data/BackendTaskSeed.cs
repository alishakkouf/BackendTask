using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Data.Models;
using BackendTask.Data.Models.Identity;
using BackendTask.Data.Models.ToDoTask;
using BackendTask.Domain.Authorization;
using BackendTask.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BackendTask.Data
{
    internal static class BackendTaskSeed
    {
        /// <summary>
        /// Seed super admin user.
        /// </summary>
        internal static async Task SeedSuperAdminAsync(BackendTaskDbContext context, RoleManager<UserRole> roleManager, UserManager<UserAccount> userManager)
        {
            var role = await context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name == StaticRoleNames.SuperAdmin);

            if (role == null)
            {
                role = new UserRole(StaticRoleNames.SuperAdmin) { IsActive = true, Description = string.Empty };
                await roleManager.CreateAsync(role);
            }

            var user = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.UserName == Constants.SuperAdminEmail);

            if (user == null)
            {
                user = new UserAccount
                {
                    UserName = Constants.SuperAdminEmail,
                    Email = Constants.SuperAdminEmail,
                    FirstName = "Super Admin",
                    LastName = "Super Admin",
                    PhoneNumber = "000000000",
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, Constants.DefaultPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Error($"Error: {error.Code} - {error.Description}");
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }
                await userManager.AddToRoleAsync(user, StaticRoleNames.SuperAdmin);
            }
        }

        internal static async Task SeedStaticRolesAsync(BackendTaskDbContext context, RoleManager<UserRole> roleManager)
        {
            foreach (var rolePermission in StaticRolePermissions.Roles)
            {
                var role = await roleManager.Roles.Include(x => x.Claims)
                                                  .FirstOrDefaultAsync(x =>
                    x.Name == rolePermission.Role);

                if (role == null)
                {
                    role = new UserRole(rolePermission.Role)
                    {
                        IsActive = true,
                        Description = string.Empty
                    };

                    await roleManager.CreateAsync(role);

                    var newRole = await roleManager.Roles.Include(x => x.Claims)
                                  .FirstOrDefaultAsync(x => x.Name == rolePermission.Role);

                    //Add static role permissions to db
                    foreach (var permission in rolePermission.Permissions)
                    {
                        await roleManager.AddClaimAsync(role,
                            new Claim(Constants.PermissionsClaimType, permission.Permission));
                    }

                    continue;
                }

                if (rolePermission.Role == StaticRoleNames.Administrator)
                {
                    var dbRoleClaims = await roleManager.GetClaimsAsync(role);

                    //Remove any claim in db and not in static role permissions.
                    foreach (var dbPermission in dbRoleClaims.Where(x => x.Type == Constants.PermissionsClaimType &&
                                                                         !rolePermission.Permissions.Any(p => p.Permission == x.Value)))
                    {
                        await roleManager.RemoveClaimAsync(role, dbPermission);
                    }

                    //Add static role permissions to db if they don't already exist.
                    foreach (var permission in rolePermission.Permissions)
                    {
                        if (!dbRoleClaims.Any(x => x.Type == Constants.PermissionsClaimType && x.Value == permission.Permission))
                        {
                            await roleManager.AddClaimAsync(role,
                                new Claim(Constants.PermissionsClaimType, permission.Permission));
                        }
                    }
                }
                await roleManager.UpdateAsync(role);
            }


        }

        internal static async Task SeedCustomePermissionsAsync(BackendTaskDbContext context)
        {
            var data = await context.CustomePermissions.ToListAsync();

            context.CustomePermissions.RemoveRange(data);

             var permissions = Permissions.All;

                var newData = permissions.SelectMany(x => new List<CustomePermission>
                {
                    new CustomePermission
                    {
                        Permission = x.Permission,
                        PermissionLabel = x.PermissionLabel,
                        Subject = x.Subject,
                        Action = x.Action
                    }
                }).ToList();

                await context.CustomePermissions.AddRangeAsync(newData);




            await context.SaveChangesAsync();
        }

        internal static async Task SeedDefaultUserAsync(BackendTaskDbContext context,
            UserManager<UserAccount> userManager,
            RoleManager<UserRole> roleManager, string adminPassword)
        {
            var adminRole = await roleManager.Roles
                .FirstOrDefaultAsync(x => x.Name == StaticRoleNames.Administrator);

            var user = await userManager.Users.Where(x => x.UserName != Constants.SuperAdminEmail)
            .FirstOrDefaultAsync();

            if (user == null && adminRole != null)
            {
                user = new UserAccount
                {
                    UserName = "user@Admin.org",
                    Email = "user@Admin.org",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "0123456789",
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Error($"Error: {error.Code} - {error.Description}");
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }

                await userManager.AddToRoleAsync(user, StaticRoleNames.Administrator);
            }            
        }

        internal static async Task SeedDefaultGuestAsync(BackendTaskDbContext context,
    UserManager<UserAccount> userManager,
    RoleManager<UserRole> roleManager, string adminPassword)
        {
            var adminRole = await roleManager.Roles
                .FirstOrDefaultAsync(x => x.Name == StaticRoleNames.Guest);

            var user = await userManager.Users.Where(x => x.UserName != Constants.SuperAdminEmail &&
                                                          x.FirstName != "Admin")
                                              .FirstOrDefaultAsync();

            if (user == null && adminRole != null)
            {
                user = new UserAccount
                {
                    UserName = "user@guest.org",
                    Email = "user@guest.org",
                    FirstName = "guest",
                    LastName = "guest",
                    PhoneNumber = "0123456789",
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Error($"Error: {error.Code} - {error.Description}");
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }

                await userManager.AddToRoleAsync(user, StaticRoleNames.Guest);
            }
        }

        internal static async Task SeedDefaultCategoryAsync(BackendTaskDbContext context)
        {
            var defaultCategory = await context.Category.FirstOrDefaultAsync();

            if (defaultCategory == null)
            {
                var toBeInserted = new Category
                {
                    Name = "Default"
                };

                await context.Category.AddAsync(toBeInserted);
                await context.SaveChangesAsync();
            }
        }
    }
}
