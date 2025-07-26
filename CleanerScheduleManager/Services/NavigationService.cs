using CleanerScheduleManager.Views;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using CleanerScheduleManager.Services.Interfaces;

namespace CleanerScheduleManager.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _frame;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider) => this._serviceProvider= serviceProvider;

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<T>() where T : Page
        {
            if (_frame != null)
            {
                var page = _serviceProvider.GetRequiredService<T>();
                _frame.Navigate(page);
            }
        }
    }
}