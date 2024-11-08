using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ProductivitiesModel : NangXuat
    {
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int productId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceCM { get; set; }
        public int? IdDenNangSuat { get; set; }
        public int LaborsBase { get; set; }

        public List<NangSuat_CumLoi> Errors { get; set; }
        public int OrderIndex { get; set; }
        public int LK_TH { get; set; }
        public int LK_BTP { get; set; }
        public int LK_TC { get; set; }
        public int ProductionPlans { get; set; }
        public double TimePerCommo { get; set; }
    }
}