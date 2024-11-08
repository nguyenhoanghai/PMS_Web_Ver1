using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLPlayVideo
    {
        private QLNSEntities db;
        public BLLPlayVideo()
        {
            db = new QLNSEntities();
        }

        public VideoScheduleModel GetVideoSchedule(int lineId)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;
                var obj = db.P_PlayVideoShedule.Where(x => !x.IsDeleted && x.LineId == lineId && x.IsActive &&   x.TimeStart <= time&& time < x.TimeEnd).Select(x => new VideoScheduleModel() { Id = x.Id, LineId = x.LineId, TimeStart = x.TimeStart, TimeEnd = x.TimeEnd }).FirstOrDefault();
                if (obj != null)
                    obj.Detail.AddRange(db.P_PlayVideoSheduleDetail.Where(x => !x.IsDeleted && !x.P_VideoLibrary.IsDeleted && x.VideoSheduleId == obj.Id).Select(x => new SheduleDetailModel()
                    {
                        Index = x.OrderIndex,
                        Name = x.P_VideoLibrary.Name,
                        Path = x.P_VideoLibrary.Path,
                        Type = x.P_VideoLibrary.Type
                    }).OrderBy(x => x.Index).ToList());
                return obj;
            }
            catch (Exception)
            {
            }
            return null;
        }

    }
}