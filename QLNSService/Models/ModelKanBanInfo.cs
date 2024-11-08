using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelKanBanInfo
    {
        public string LineName { get; set; }
        public string ProductName { get; set; }
        public int BTPOnDay { get; set; }
        public int BTPTotal { get; set; }
        public string BTPBQ { get; set; }
        public string StatusColor { get; set; }
        public string LightBTPConLai { get; set; }
        public int LK_BTP_HC { get; set; }
        public int LK_BTP { get; set; }
        public int ProductionPlans { get; set; }
        public int BTPBinhQuan { get; set; }
        public int BTPInLine { get; set; }
        public List<BTP_HCStructureModel> BTPHC_Structs { get; set; }
        public ModelKanBanInfo()
        {
            BTPHC_Structs = new List<BTP_HCStructureModel>();
        }
    }
}