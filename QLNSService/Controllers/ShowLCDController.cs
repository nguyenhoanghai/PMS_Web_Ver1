using QLNSService.BLL;
using QLNSService.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class ShowLCDController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LCDError()
        {
            return View();
        }

        public ActionResult LCDGeneral()
        {
            return View();
        }

        public ActionResult LCDKanBan()
        {
            return View();
        }

        public ActionResult LCDShow(string listTableTypeId, string jGroup)
        {
            var arrTableTypeId = listTableTypeId.Split(new char[] { ',' });
            ViewBag.ListTableTypeId = arrTableTypeId.ToList();
            ViewBag.jGroup = jGroup;

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

        public JsonResult GetLCDConfigInfo()
        {
            try
            {
                var bllLCDConfig = new BLLLCDConfig();
                var listConfig = bllLCDConfig.GetConfig();
                return Json(listConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
