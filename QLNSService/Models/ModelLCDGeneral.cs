using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelLCDGeneral
    {        
        public List<ModelMorthProductivity> ListLineMorthProductivity { get; set; }
        public List<WorkingTimeModel> ListHoursProductivity { get; set; }
        public string Morth { get; set; }
        public int THThang { get; set; }
        public int KHThang { get; set; }
        public double DoanhThuTHThang { get; set; }
        public double DoanhThuKHThang { get; set; }
        public double NhipSanXuatTH { get; set; }
        public double NhipThucTeKH { get; set; }
        public ModelLCDGeneral()
        {
            this.ListLineMorthProductivity = new List<ModelMorthProductivity>();
            this.ListHoursProductivity = new List<WorkingTimeModel>();
        }
    }
}