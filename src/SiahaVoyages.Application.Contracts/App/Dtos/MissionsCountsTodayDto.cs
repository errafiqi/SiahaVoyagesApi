using System;
using System.Collections.Generic;
using System.Text;

namespace SiahaVoyages.App.Dtos
{
    public class MissionsCountsTodayDto
    {
        public int AffectedMissionsCount { get; set; }

        public int CompletedMissionsCount { get; set; }

        public int InProgressMissionsCount { get; set; }
    }
}
