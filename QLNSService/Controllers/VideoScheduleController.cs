using QLNSService.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class VideoScheduleController : Controller
    { 


        public JsonResult GetVideoSchedule(int lineId)
        {
            var jsonResult = new JsonResult();
            try
            {
                var bll = new BLLPlayVideo(); 
                jsonResult.Data = bll.GetVideoSchedule(lineId); ;
            }
            catch (Exception ex)
            {
            }
            return Json(jsonResult);
        }

    }
}
