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

        public int ClientId { get; set; }

        public int CleanerId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public TimeSpan Duration { get; set; }

        public Enums.TaskStatus Status { get; set; }
    }
}
