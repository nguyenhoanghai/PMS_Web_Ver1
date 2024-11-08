using QLNSService.BLL;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace QLNSService.Controllers
{
    public class LCDConfigController : ApiController
    {
        public object Post([FromBody]ModelGetLCDConfig modelGetLCDConfig)
        {
            try
            {
                BLLLCDConfig bllLCDConfig = new BLLLCDConfig();
                switch (modelGetLCDConfig.ConfigTypeId)
                {
                    case 1: return bllLCDConfig.GetTableLayoutPanel(modelGetLCDConfig.TableTypeId);
                    case 2: return bllLCDConfig.GetPanel(modelGetLCDConfig.TableTypeId);
                    case 3: return bllLCDConfig.GetLabelArea(modelGetLCDConfig.TableTypeId);
                    case 4: return bllLCDConfig.GetLabelForPanelContent(modelGetLCDConfig.TableTypeId);
                    case 5: return bllLCDConfig.GetConfig();
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
