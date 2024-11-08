using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class LineWorkingShiftModel : P_LineWorkingShift
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string LineName { get; set; }
        public string ShiftName { get; set; }
        public string Time { get; set; }
    }
}