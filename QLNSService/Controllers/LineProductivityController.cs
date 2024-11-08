using QLNSService.BLL;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace QLNSService.Controllers
{
    public class LineProductivityController : ApiController
    {
        private BLLLCDConfig bllLCDConfig;
        public LineProductivityController()
        {
            bllLCDConfig = new BLLLCDConfig();
        }

        public bool Get()
        {
            return false;
        }

        private void LoadLCDConfig()
        {
            try
            {
                var listLCDConfig = bllLCDConfig.GetConfig();
                if (listLCDConfig != null && listLCDConfig.Count > 0)
                { }
                    
            }
            catch (Exception ex)
            {

                
            }
        }

        

    }
}
