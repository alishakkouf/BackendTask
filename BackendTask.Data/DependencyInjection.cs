using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Data.Providers;
using BackendTask.Domain;
using BackendTask.Domain.Contract.TasksContract;
using BackendTask.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BackendTask.Data.Providers;
using BackendTask.Domain.Contract.UserContract;
using BackendTask.Data.Models.Identity;

namespace BackendTask.Data
{
    public static class DependencyInjection
    {
        const string ConnectionStringName = "DefaultConnection";
        const bool SeedData = true;

        public static IServiceCollection ConfigureDataModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BackendTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStringName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddProviders();

            return services;
        } 

        public static async Task MigrateAndSeedDatabaseAsync(this IApplicationBuilder builder)
        {
            var scope = builder.ApplicationServices.CreateAsyncScope();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<BackendTaskDbContext>();

                if (context.Database.GetPendingMigrations().Count() > 0)
                {
                    await context.Database.MigrateAsync();
                }

                if (SeedData)
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserAccount>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
                    await BackendTaskSeed.SeedSuperAdminAsync(context, roleManager, userManager);
                    await BackendTaskSeed.SeedStaticRolesAsync(context, roleManager);
                    await BackendTaskSeed.SeedCustomePermissionsAsync(context);
                    await BackendTaskSeed.SeedDefaultCategoryAsync(context);
                    await BackendTaskSeed.SeedDefaultUserAsync(context, userManager, roleManager, Constants.DefaultPassword);
                    await BackendTaskSeed.SeedDefaultGuestAsync(context, userManager, roleManager, Constants.DefaultPassword);

                    //await AbhaSeed.SeedDefaultSettingsAsync(context);                  

                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<BackendTaskDbContext>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }


        private static void AddProviders(this IServiceCollection services)
        {
            services.AddScoped<ITaskProvider, TasksProvider>();
            services.AddScoped<IAccountProvider, AccountProvider>();
        }
    }
}
