using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Data;

namespace QLNSService.Models
{
    public class CompletionDailyModel : P_CompletionPhase_Daily
    {
        public string PhaseName { get; set; }
        public int CommoId { get; set; }
        public string CommoName { get; set; }
        public int OrderIndex { get; set; }
    }
}