using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Models
{
    public class VideoScheduleModel : P_PlayVideoShedule
    {
        public List<SheduleDetailModel> Detail { get; set; }
        public VideoScheduleModel()
        {
            Detail = new List<SheduleDetailModel>();
        }
    }

    public class SheduleDetailModel {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }

    }
}