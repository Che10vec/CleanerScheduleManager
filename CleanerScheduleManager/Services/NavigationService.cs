using CleanerScheduleManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CleanerScheduleManager.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _frame;

        private readonly Dictionary<string, Func<Page>> _pageFactory = new()
        {
            { "Cleaner", () => new CleanerView() },
            { "Client", () => new ClientView() },
            { "Task", () => new TaskView() }
        };

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string pageKey)
        {
            if (_frame != null && _pageFactory.TryGetValue(pageKey, out var factory))
            {
                _frame.Navigate(factory.Invoke());
            }
        }
    }
}
