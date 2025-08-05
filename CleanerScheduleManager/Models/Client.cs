using CleanerScheduleManager.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.Models
{
    public class Client : Person
    {
        public string? Address { get; set; } = string.Empty;

        public string? Phone { get; set; } = string.Empty;
    }
}
