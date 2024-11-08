using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class ModelProductivity
    {
        public string ProductName { get; set; }
        public string LineName { get; set; }
        public int LDDB { get; set; }
        public int LDTT { get; set; }
        public int SLKH { get; set; }
        public int SLCL { get; set; }
        public int DMN { get; set; }
        public int DMHour { get; set; }
        public int KCS { get; set; }
        public int KCSHour { get; set; }
        public int TC { get; set; }
        public int Error { get; set; }
        public int ErrHour { get; set; }
        public int TCHour { get; set; }
        public int BTPHour { get; set; }
        public int BTP { get; set; }
        public double BTPInLine { get; set; }
        public double BTPInLine_BQ { get; set; }
        public double DoanhThuBQ { get; set; }
        public double DoanhThuBQ_T { get; set; }
        public double DoanhThu { get; set; }
        public double DoanhThu_T { get; set; }
        public double DoanhThuKH_T { get; set; }
        public double KCS_KH_T { get; set; }
        public double DoanhThuDM { get; set; }
        public double DoanhThuDM_T { get; set; }
        public double KCSKH_T { get; set; }
        public double KCS_T { get; set; }
        public double ThuNhapBQ { get; set; }
        public double ThuNhapBQ_T { get; set; }
        public double SLKHToNow { get; set; }
        public string KieuHienThiNangSuatGio { get; set; }
        public double NhipSX { get; set; }
        public double NhipTT { get; set; }
        public double NhipTC { get; set; }
        public string tiLeThucHien { get; set; }
        public string nangSuatGioTruoc { get; set; }
        public string nangSuatGioHienTai { get; set; }
        public string tongNangSuat { get; set; }

        public int Hour_ChenhLech_Day { get; set; }
        public int Hour_ChenhLech { get; set; }
        public int KCS_QuaTay { get; set; }
        public int LK_KCS_QuaTay { get; set; }

        public int LK_TC { get; set; }
        public int TiLeLoi_H { get; set; }
        public int TiLeLoi_D { get; set; }
        public int LK_KCS { get; set; }
        public int LK_BTP { get; set; }
        public string mauDen { get; set; }
        public List<WorkingTimeModel> listWorkHours { get; set; }
        public double Lean { get; set; }
        public string NSHienTai { get; set; }      
        public int HieuSuat { get; set; }

    }
}