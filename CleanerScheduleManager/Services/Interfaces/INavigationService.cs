using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CleanerScheduleManager.Services.Interfaces
{
    public interface INavigationService
    {
        void SetFrame(Frame frame);
        void NavigateTo<T>() where T : Page;
    }
}