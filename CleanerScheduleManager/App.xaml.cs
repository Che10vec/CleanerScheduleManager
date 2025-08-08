using CleanerScheduleManager.DependencyInjection;
using CleanerScheduleManager.ViewModels;
using CleanerScheduleManager.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
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
            try
            {
                await AppHost.StartAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to start application host", ex);
            }

            try
            {
                _persistableViewModels = AppHost.Services
                    .GetServices<IPersistable>()
                    .OrderBy(vm => vm is TaskViewModel ? 1 : 0)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to resolve view models", ex);
            }

            try
            {
                foreach (var vm in _persistableViewModels)
                {
                    await vm.LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to load application data", ex);
            }

            try
            {
                var mainWindow = AppHost
                    .Services
                    .GetRequiredService<MainWindow>();

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize main window", ex);
            }

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                foreach (var vm in _persistableViewModels)
                {
                    await vm.SaveDataAsync();
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to save application data", ex);
            }

            try
            {
                await AppHost.StopAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to stop application host", ex);
            }

            base.OnExit(e);
        }
    }
}
