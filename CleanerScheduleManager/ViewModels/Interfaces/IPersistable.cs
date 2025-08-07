using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.ViewModels.Interfaces
{
    public interface IPersistable
    {
        Task LoadDataAsync();
        Task SaveDataAsync();
    }
}
