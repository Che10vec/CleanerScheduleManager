using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanerScheduleManager.Models.Enums;

namespace CleanerScheduleManager.Models
{
    public class CleaningTask
    {
        public int TaskId { get; set; }

        public Client? Client { get; set; }

        public Cleaner? Cleaner { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public TimeSpan? Duration { get; set; }

        public Enums.TaskStatus Status { get; set; }
    }
}
