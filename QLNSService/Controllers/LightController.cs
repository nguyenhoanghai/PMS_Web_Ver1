using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class LightController : Controller
    { 

        public ActionResult TiLeCat()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Gets( int lightType)
        {
            var tile = BLLLightPercent.Instance.GetAll(lightType);
            return Json(tile); 
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var tile = BLLLightPercent.Instance.Delete(Id);
            return Json(tile);
        }

        [HttpPost]
        public JsonResult Save(LightPercentModel model)
        { 
            return Json( BLLLightPercent.Instance.InsertOrUpdate(model));
        }

    }
}
