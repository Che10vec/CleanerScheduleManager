using CleanerScheduleManager.Services;
using CleanerScheduleManager.ViewModels;
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
                    services.AddSingleton<CleanerViewModel>();
                    services.AddSingleton<ClientViewModel>();
                    services.AddSingleton<TaskViewModel>();

                    // UI
                    services.AddSingleton<MainWindow>();
                });
    }
}
