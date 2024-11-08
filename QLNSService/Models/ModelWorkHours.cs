using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class WorkingTimeModel
    {
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public int IntHours { get; set; }
        public string Name { get; set; }
        public float TC { get; set; }
        public double NormsHour { get; set; }
        public float KCS { get; set; }
        public float Lean { get; set; }
        public float Error { get; set; }
        public float BTP { get; set; }
        public int LineId { get; set; }
        public string HoursProductivity { get; set; }
        public string HoursProductivity_1 { get; set; }
        public bool IsShow { get; set; }
    }
}