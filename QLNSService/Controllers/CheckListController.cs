using QLNSService.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class CheckListController : Controller
    { 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CollectionPage_Partial()
        {
            return PartialView();
        }

        public JsonResult GetData()
        {
            try
            { 
                var rs = new JsonResult();
                rs.Data = BLLCheckList.Get_Collection( );
                return Json(rs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult StaplePartial()
        {
            return PartialView();
        }

        public JsonResult GetByJGroupId(int JGroupId, int userId)
        {
            try
            {
                var rs = new JsonResult();
                rs.Data = BLLCheckList.Get_JGroupId(JGroupId, userId);
                return Json(rs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       

    }
}
