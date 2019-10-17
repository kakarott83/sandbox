using System;

namespace Cic.One.DTO
{
    public class MRecurrence
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? NumberOfOccurrences { get; set; }

        public int Period { get; set; }

        public int Intervall { get; set; }

        public int DayIndex { get; set; }

        public int MonthIndex { get; set; }

        public int? DayWeekIndex { get; set; }

        public bool OnMondays { get; set; }

        public bool OnTuesdays { get; set; }

        public bool OnWednesdays { get; set; }

        public bool OnThursdays { get; set; }

        public bool OnFridays { get; set; }

        public bool OnSaturdays { get; set; }

        public bool OnSundays { get; set; }

        public bool OnWorkdays { get; set; }
    }
}