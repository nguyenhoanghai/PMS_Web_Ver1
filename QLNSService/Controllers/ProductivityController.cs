using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using QLNSService.Models;
using QLNSService.BLL;
using QLNSService.Enum;
using PMS.Business.Web;

namespace QLNSService.Controllers
{
    public class ProductivityController : Controller
    {
        public JsonResult GetProductivityByLineId(int Id, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            var jsonResult = new JsonResult();
            try
            {
                var config = BLLApplicationConfig.Instance.GetConfig(eConfigName.CalculateNormsdayType.ToUpper(), 11);
                var bllProductivity = new BLLProductivity();
                var model = !string.IsNullOrEmpty(config) && config == "0" ? bllProductivity.GetProductivityByLineId_KTC(Id, tableTypeId, hienThiNSGio, TimesGetNS, KhoangCachGetNSOnDay) : bllProductivity.GetProductivityByLineId(Id, tableTypeId, hienThiNSGio, TimesGetNS, KhoangCachGetNSOnDay);
                jsonResult.Data = model;
            }
            catch (Exception ex)
            { }
            return Json(jsonResult);
        }

        public JsonResult GetProductivityByLineId_New(int Id, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            var jsonResult = new JsonResult();
            try
            {
                var bllProductivity = new BLLProductivity();
                var model = bllProductivity.GetProductivityByLineId_new(Id, tableTypeId, hienThiNSGio, TimesGetNS, KhoangCachGetNSOnDay);
                jsonResult.Data = model;
            }
            catch (Exception ex)
            {
            }
            return Json(jsonResult);
        }

        public JsonResult GetTotalProductivity(List<int> listLineId, int tableTypeId, bool IsKanbanAcc)
        {
            try
            {
                BLLProductivity bllProductivity = new BLLProductivity();
                JsonResult jsonResult = new JsonResult();
                ModelLCDGeneral model = bllProductivity.GetGeneralInfo(listLineId, tableTypeId, IsKanbanAcc);
                jsonResult.Data = model;
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetKanBanInfo(List<int> listLineId, int tableTypeId,bool includeBTPHC)
        {
            try
            { 
                JsonResult jsonResult = new JsonResult(); 
                jsonResult.Data = BLLLCD.Instance.GetKanBanLCD(listLineId, tableTypeId, includeBTPHC); 
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetInforLCDTH_New(List<int> listLineId, int tableTypeId )
        {
            try
            { 
                JsonResult jsonResult = new JsonResult();
                jsonResult.Data = BLLLCD.Instance.GetTongHopLCD(listLineId, tableTypeId, 0, 0, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HoanThanhId"].ToString()));  
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LCDNS_New_Partial()
        {
            return PartialView();
        }

        public ActionResult LCDProductivity_Partial()
        {
            return PartialView();
        }
        public ActionResult LCDKanBan_Partial()
        {
            return PartialView();
        }
        public ActionResult LCDTongHopN_Partial()
        {  
            var bllconfig = new BLLLCDConfig();
            var config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.WebPageHeight));
            ViewData["Height"] = config != null ? config.Value : "650";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.Interval_ChangeLCD));
            ViewData["timeChangeLCD"] = config != null ? config.Value : "2000";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.LCDTongHop_Paging));
            ViewData["paging_TH"] = config != null ? config.Value : "5";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.LCDKanBan_Paging));
            ViewData["paging_KB"] = config != null ? config.Value : "5";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.IntervalShow));
            ViewData["IntervalShow"] = config != null ? config.Value : "1000";

            return View();
        }

        public ActionResult SH_Pro_Partial ()
        {
            var bllconfig = new BLLLCDConfig();
            var config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.WebPageHeight));
            ViewData["Height"] = config != null ? config.Value : "650";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.Interval_ChangeLCD));
            ViewData["timeChangeLCD"] = config != null ? config.Value : "2000";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.LCDTongHop_Paging));
            ViewData["paging_TH"] = config != null ? config.Value : "5";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.LCDKanBan_Paging));
            ViewData["paging_KB"] = config != null ? config.Value : "5";

            config = bllconfig.GetConfig().FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.IntervalShow));
            ViewData["IntervalShow"] = config != null ? config.Value : "1000";

            return View();
        }

        public ActionResult LCDCompletion_Partial()
        {
            return PartialView();
        }
        public ActionResult ProOfLineEachHours_Partial()
        {
            return PartialView();
        }

        public ActionResult ProOfLineOfDay_Partial()
        {
            return PartialView();
        }

        public ActionResult ProOfLineEachHours_Output_Partial()
        {
            return PartialView();
        }

        public ActionResult ProOfLineOfDay_Output_Partial()
        {
            return PartialView();
        }

        public ActionResult LCDProductivityCollection_Partial()
        {
            return PartialView();
        }

        public JsonResult GetProductivitiesForDrawChart(List<int> lineIds, DateTime date, bool isOneLine, bool IsProductOutput)
        {
            try
            {
                var bllPro = new BLLProductivity();
                var jsonResult = new JsonResult();
                jsonResult.Data = bllPro.GetProductivities(lineIds, date, isOneLine, IsProductOutput);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult InsertProductivity(InsertProductivityModel product)
        {
            try
            {
                var bllPro = new BLLProductivity();
                var jsonResult = new JsonResult();
                jsonResult.Data = bllPro.InsertProductivity(product);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LCDNS_Partial()
        {
            return PartialView();
        }

        public ActionResult LCDNS_Collection_Partial()
        {
            return PartialView();
        }

        public JsonResult GetLCDInfo(int Id, int tableId, int configTypeId)
        {
            try
            {
                var bllPro = new BLLProductivity();
                var jsonResult = new JsonResult();
                jsonResult.Data = bllPro.GetLCDInfo_New(tableId, Id);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetCheckListConfig(int tableTypeId)
        {
            try
            {
                var bllPro = new BLLProductivity();
                var jsonResult = new JsonResult();
                jsonResult.Data = bllPro.GetCheckListConfig(tableTypeId);
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Data for LCD Hoan Tat
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLCD_HT_Data()
        {
            try
            {
                var bllPro = new BLLProductivity();
                var rs = new JsonResult();
                rs.Data = bllPro.Get_LCD_Completion();
                return Json(rs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult HoanTatLCD1( )
        {
            return PartialView("HoanTatLCD1" );
        }
        public ActionResult HoanTatLCD2( )
        {
            return PartialView("HoanTatLCD2" );
        }
        public ActionResult HoanTatLCD3( )
        {
            return PartialView("HoanTatLCD3" );
        }

    }
}
