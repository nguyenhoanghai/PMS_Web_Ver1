using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelMorthProductivity
    {
        public string LineName { get; set; }
        public string CommoName { get; set; }
        public int THNgay { get; set; }
        public int KHNgay { get; set; }
        public int THThang { get; set; }
        public int KHThang { get; set; }
        public double DoanhThuTH { get; set; }
        public double DoanhThuKH { get; set; }
        public double TiLeTH { get; set; }

    }
}