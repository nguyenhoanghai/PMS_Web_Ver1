using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http; 
using PMS.Business.Web;
using PMS.Business.Web.Models;

namespace QLNSService.Controllers
{
    public class ApiPMSController : ApiController
    {

        public object HoanTatLCD(  string lineId)
        {
            var a = BLLLCD.Instance.GetHoanTatLCD(lineId.Split(',').Select(x=>Convert.ToInt32(x)).ToList());
            return a;
        }

    }
}
