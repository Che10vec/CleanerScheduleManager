using CleanerScheduleManager.Models.Base;
using CleanerScheduleManager.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanerScheduleManager.Models
{
    public class Cleaner : Person
    {
        public CleanerSkillLevel SkillLevel { get; set; }

        public bool IsAvailable { get; set; }
    }
}
