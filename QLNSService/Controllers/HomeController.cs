using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLNSService.BLL;
using System.Configuration;
using QLNSService.Models;

namespace QLNSService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult TrangChu()
        { 
            return View();
        }

        public ActionResult Index()
        {
            bool IsUseAccount = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsUseAccount"], out IsUseAccount);
           
            ViewBag.jGroup =new List<SelectListItem>();// IsUseAccount ? BLLCheckList.GetJobGroupSelect() : new List<SelectListItem>();
            return View();            
        }

        public JsonResult GetBTPHC_Struct( )
        {
            try
            { 
                var jsonResult = new JsonResult();
                jsonResult.Data = BLLBTP_HCStructure.Instance.Gets( );
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
