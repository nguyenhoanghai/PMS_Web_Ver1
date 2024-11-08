using PMS.Business;
using PMS.Business.Web;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class AssignmentController : Controller
    { 

        public ActionResult Index()
        {
            ViewBag.Lines = new SelectList(BLLLine.GetLines(Convert.ToInt32(ConfigurationManager.AppSettings["FloorId"].ToString())), "MaChuyen", "TenChuyen", "Chọn chuyền");
            ViewBag.Products = new SelectList(BLLCommodity.Gets(Convert.ToInt32(ConfigurationManager.AppSettings["FloorId"].ToString()),1), "MaSanPham", "TenSanPham", "Chọn sản phẩm");
            return View();
        }

        [HttpPost]
        public JsonResult Gets(int lineId )
        {
            return Json(BLLAssignmentForLine.Instance.GetDataForGridView(lineId)); 
        }
         

        [HttpPost]
        public JsonResult Save(Chuyen_SanPham model)
        {
            return Json(BLLAssginForWeb.Instance.InsertOrUpdate(model));
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            return Json(BLLAssignmentForLine.Instance.Delete(Id));
        }

    }
}
