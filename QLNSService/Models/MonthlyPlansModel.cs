using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Data;

namespace QLNSService.Models
{
    public class MonthlyPlansModel : P_MonthlyProductionPlans
    {
        public double Price { get; set; }
        public double PriceCM { get; set; }
        public int LineId { get; set; }
    }
}