using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ChuyenSanPhamModel : Chuyen_SanPham
    {
        public string TenSanPham { get; set; }

        public double? DonGiaSanXuat { get; set; }
        public string TenChuyen { get; set; }
    }
}