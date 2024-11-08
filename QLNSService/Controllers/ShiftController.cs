using QLNSService.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class ShiftController : Controller
    {
        private BLLShift bllShift = new BLLShift();
        public ShiftController()
        {
            this.bllShift = new BLLShift();
        }
        public JsonResult CountWorkHoursInDayOfLine(int lineId)
        {
            try
            {
                var countWorkHours = bllShift.CountWorkHoursInDayOfLine(lineId);
                return Json(countWorkHours);
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public JsonResult CountWorkHoursMaxOfLines(List<int> listLineId)
        {
            try
            {
                var maxWorkHours = bllShift.CountWorkHoursMaxOfLines(listLineId);
                return Json(maxWorkHours);
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public JsonResult GetWorkHoursInDayOfLine(int lineId)
        {
            try
            {
                var workHours = bllShift.GetListWorkHoursOfLineByLineId(lineId);
                return Json(workHours);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
