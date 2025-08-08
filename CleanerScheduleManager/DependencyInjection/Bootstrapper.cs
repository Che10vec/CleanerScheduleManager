using CleanerScheduleManager.Services;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.ViewModels;
using CleanerScheduleManager.ViewModels.Interfaces;
using CleanerScheduleManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.DependencyInjection
{
    public static class Bootstrapper
    {
        public static IHostBuilder CreateHostBuilder() =>
            Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Services
                    services.AddSingleton<IDataService, JsonDataService>();
                    services.AddSingleton<INavigationService, NavigationService>();

                    // ViewModels
                    services.AddSingleton<MainViewModel>();
                    services.AddPersistableViewModel<CleanerViewModel>();
                    services.AddPersistableViewModel<ClientViewModel>();
                    services.AddPersistableViewModel<TaskViewModel>();

                    // UI
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainView>();
                    services.AddTransient<CleanerView>();
                    services.AddTransient<ClientView>();
                    services.AddTransient<TaskView>();
                });
        public static IServiceCollection AddPersistableViewModel<T>(this IServiceCollection services)
            where T : class, IPersistable
        {
            services.AddSingleton<T>();
            services.AddSingleton<IPersistable>(sp =>
            {
                try
                {
                    return sp.GetRequiredService<T>();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to resolve view model '{typeof(T).Name}'", ex);
                }
            });
            return services;
        }
    }
}