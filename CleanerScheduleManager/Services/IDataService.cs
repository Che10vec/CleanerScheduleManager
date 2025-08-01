using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.Services
{
    public interface IDataService
    {
        Task<List<T>> LoadAsync<T>(string path);
        Task SaveAsync<T>(IEnumerable<T> items, string path);
    }
}
