using BackendTask.Domain.Contract.TasksContract;
using BackendTask.Domain.Contract.UserContract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendTask.Manager
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureManagerModule(this IServiceCollection services,
                                                                     IConfiguration configuration)
        {
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IToDoTaskManager, ToDoTaskManager>();

            return services;
        }
    }
}
