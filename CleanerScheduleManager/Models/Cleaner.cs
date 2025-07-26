using CleanerScheduleManager.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanerScheduleManager.Models
{
    public class Cleaner : Person
    {
        public string SkillLevel { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}
