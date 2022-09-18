using System;
using System.Collections.Generic;
using System.Text;

namespace SiahaVoyages.App.Dtos
{
    public class MissionsCountThisWeekPerDayDto
    {
        public DayOfWeek DayOfWeek { get; set; }

        public int Count { get; set; }
    }
}
