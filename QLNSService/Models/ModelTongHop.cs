using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelTongHop : P_PM_JobGroup
    {
        public string OrderName { get; set; }
        public string CustomerName { get; set; }
        public string JobGroupName { get; set; }
        public string Status { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
    }
}