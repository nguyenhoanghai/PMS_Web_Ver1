using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class NangSuatModel : NangXuat
    {
        public int productId { get; set; }
        public string ProductName { get; set; }
        public double? ProductPrice { get; set; }
        public double ProductPriceCM { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int? IdDenNangSuat { get; set; }
    }
}