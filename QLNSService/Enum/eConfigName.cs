using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Enum
{
    public static class eConfigName
    {
        public const string GetBTPInLineByType = "GETBTPINLINEBYTYPE";
        public const string WebPageHeight = "WEBPAGEHEIGHT";
        public const string LCDTongHop_Paging = "SODONGHIENTHILCDTONGHOP";
        public const string LCDKanBan_Paging = "SODONGHIENTHILCDKANBAN";
        public const string Interval_VerticalAutoScroll_Tick = "THOIGIANNHAYCHUYEN";
        public const string Interval_ChangeLCD = "THOIGIANCHUYENLCD";
        public const string PagingLCDCheckList_TH = "PAGINGLCDCHECKLIST_TH";
        public const string TimerTickCheckList_TH = "TIMERTICKCHECKLIST_TH";
        public const string PagingLCDCheckList_CT = "PAGINGLCDCHECKLIST_CT";
        public const string TimerTickCheckList_CT = "TIMERTICKCHECKLIST_CT";

        /// <summary>
        /// số lần lấy thông tin năng suất trong ngày của chuyền
        /// </summary>
        public const string TimesGetNSInDay = "TIMESGET_NSINDAY";
        public const string TypeShowProductivitiesPerHour = "HIENTHINSGIO";
        /// <summary>
        /// khoảng thời gian cách nhau của lần lấy thông tin năng suất trong ngày của chuyền
        /// </summary>
        public const string KhoangCachLayNangSuat = "KHOANGCACHLAYNANGSUAT";
       
        public const string ChieuCaoDong_LCDNS_New = "CHIEUCAODONG_LCDNS_NEW";
        public const string SoDongHienThiLCDNS_New = "SODONGHIENTHILCDNS_NEW";
        public const string ThoiGianCuonLCDNS_New = "THOIGIANCUONLCDNS_NEW";
        /// <summary>
        /// cấu hinh hien thi năng suat gio 
        /// </summary>
        public const string NSG_Formula = "NSG_FORMULA";

        public const string ChieuCaoDong_LCDKanBan = "CHIEUCAODONG_LCDKANBAN";

        public const string PagingLCD_HT = "PAGINGLCD_HT";
        public const string TimerTick_HT = "TIMERTICK_HT";
        public const string ChieuCaoDong_LCD_HT = "CHIEUCAODONG_LCD_HT";
        public const string CalculateNormsdayType = "CalculateNormsdayType";
        public const string IntervalShow = "INTERVALSHOW";  
    }
}