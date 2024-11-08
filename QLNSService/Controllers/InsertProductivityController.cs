using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLNSService.BLL;
using QLNSService.Models;
using QLNSService.Data;
using PMS.Business.Web.Models;
using PMS.Data;
using PMS.Business.Web;

namespace QLNSService.Controllers
{
    public class InsertProductivityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLineInfo(List<int> Ids)
        {
            try
            {
                var bllLine = new BLLLine();
                JsonResult jsonresult = new JsonResult();
                jsonresult.Data = bllLine.GetLines(Ids); ;
                return Json(jsonresult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetProductInfoByLine(int Id)
        {
            try
            {
                var bllLineP = new BLLLine_Product();
                JsonResult jsonresult = new JsonResult();
                jsonresult.Data = bllLineP.GetProductInfos(Id); ;
                return Json(jsonresult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetNSNgayCuaChuyen(int Id)
        {
            try
            {
                var bllLineP = new BLLLine_Product();
                JsonResult jsonresult = new JsonResult();
                jsonresult.Data = bllLineP.GetNSNgayCuaChuyen(Id);  
                return Json(jsonresult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetErrors()
        {
            try
            {
                var bllError = new BLLError();
                JsonResult jsonresult = new JsonResult();
                jsonresult.Data = bllError.GetErrors();
                return Json(jsonresult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult InsertPhaseQuantity()
        { 
            return View();
        }

        public JsonResult GetPhases(int type)
        {
            return Json(BLLPhaseInDay.Instance.GetPhases(type));
        }

        public JsonResult SavePhaseQuantity(PMS.Data.P_PhaseDaily product, int csp)
        {
            try
            {
                var jsonResult = new JsonResult();
                jsonResult.Data = BLLPhaseInDay.Instance.InsertPhaseQuantities(product, csp);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetLKPhase(int phaseId, int cspId)
        {
            try
            {
                return Json(BLLPhaseInDay.Instance.GetLKPhase(phaseId, cspId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetPhaseDayInfo(int assignId, int phaseId)
        {
            try
            { 
                return Json(BLLPhaseInDay.Instance.GetPhaseDayInfo(assignId, phaseId, DateTime.Now));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
