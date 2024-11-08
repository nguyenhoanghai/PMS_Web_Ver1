using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Data;

namespace QLNSService.Models
{
    public class DataForLCDModel
    {
        public string PageHeight { get; set; }
        public string LineName { get; set; }
        public List<ShowLCD_TableLayoutPanel> LayoutPanelConfig { get; set; } // chia do rong khung
        public List<ShowLCD_Panel> Panel_Background { get; set; }  // cau hinh mau nen
        public List<ShowLCD_LabelArea> FontStyle { get; set; }  // cau hinh font style
        public List<ShowLCD_LabelForPanelContent> BodyTitle { get; set; }

        public List<dynamic> FooterTitle { get; set; }

        public string paging { get; set; } //phan trang dung cho LCD tong hop + LCD KanBan
        public string TimeToChangeRow { get; set; } // interval change row LCD Tong Hop + LCD Kanban
        public int ShowNSType { get; set; }
        public int TimesGetNS { get; set; }
        public int KhoangCachGetNSOnDay { get; set; }
        public int rowHeight { get; set; }
        public List<string> NSG_Formula { get; set; }

        public DataForLCDModel()
        {
            LayoutPanelConfig = new List<ShowLCD_TableLayoutPanel>();
            Panel_Background = new List<ShowLCD_Panel>();
            FontStyle = new List<ShowLCD_LabelArea>();
            BodyTitle = new List<ShowLCD_LabelForPanelContent>();
            FooterTitle = new List<dynamic>();
        }
    }
}