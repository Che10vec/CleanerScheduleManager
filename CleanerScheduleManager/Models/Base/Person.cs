using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.Models.Base
{
    public abstract class Person
    {
        public int Id { get; set; }

        public string? Name { get; set; } = string.Empty;
    }
}
