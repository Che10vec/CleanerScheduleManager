using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static CleanerScheduleManager.ViewModels.Base.ViewModelBase;

namespace CleanerScheduleManager.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _frame;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider) => this._serviceProvider = serviceProvider;

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<T>() where T : Page
        {
            if (_frame == null)
                return;

            try
            {
                if (_frame.Content is Page currentPage && currentPage.DataContext is IHasPendingEdits pendingEdits)
                {
                    pendingEdits.FinalizeEdits();
                }

                var page = _serviceProvider.GetRequiredService<T>();
                _frame.Navigate(page);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to navigate to page '{typeof(T).Name}'", ex);
            }
        }
    }
}