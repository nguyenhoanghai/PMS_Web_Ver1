using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Data;

namespace QLNSService.Models
{
    public class CheckListConfigModel
    {
        public string paging_Collection { get; set; }
        public string paging_Detail { get; set; }
        public string Timer_Collection { get; set; }
        public string Timer_Detail { get; set; } 
        public string  PageHeight { get; set; }
        public string RowHeight { get; set; }
        public List<LCDConfig> LCDConfigs { get; set; }
        public CheckListConfigModel(){
           LCDConfigs = new List<LCDConfig>();
        }
    }
}