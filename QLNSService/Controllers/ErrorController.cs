using QLNSService.BLL;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class ErrorController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetErrorByLineId(int Id)
        {
            try
            {
                BLLError bllError = new BLLError();
                JsonResult jsonResult = new JsonResult();
                List<ModelGroupError> model = bllError.GetErrorOfLine(Id);
                jsonResult.Data = model;
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ErrorOfLineEachHours_Partial()
        {
            return PartialView();
        }

        public ActionResult ErrorOfLineOfDay_Partial()
        {
            return PartialView();
        }

        public JsonResult GetErrorsForDrawChart(List<int> lineIds, DateTime date, bool isOneLine)
        {
            try
            {
                var bllError = new BLLError();
                var jsonResult = new JsonResult();
                jsonResult.Data = bllError.GetErrorsForChart(lineIds, date, isOneLine);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ErrorLCD()
        {
            return PartialView();
        }
        public ActionResult LCDError_Partial()
        {
            return PartialView();
        }
    }
}
