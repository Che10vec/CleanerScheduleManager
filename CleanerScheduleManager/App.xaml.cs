using CleanerScheduleManager.DependencyInjection;
using CleanerScheduleManager.ViewModels;
using CleanerScheduleManager.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CleanerScheduleManager
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        private List<IPersistable> _persistableViewModels = new();

        public App()
        {
            AppHost = Bootstrapper
                .CreateHostBuilder()
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            _persistableViewModels = AppHost.Services
                .GetServices<IPersistable>()
                .OrderBy(vm => vm is TaskViewModel ? 1 : 0)
                .ToList();

            foreach (var vm in _persistableViewModels)
            {
                await vm.LoadDataAsync();
            }

            var mainWindow = AppHost
                .Services
                .GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            foreach (var vm in _persistableViewModels)
            {
                await vm.SaveDataAsync();
            }

            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }
}
