using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Enum
{
    public enum eShowNSType
    {
        TH_DM_FollowHour = 0,
        PercentTH_FollowHour = 1,
        PercentTH_OnDay = 2,
        TH_TC_FollowHour = 3,
        TH_Err_FollowHour = 4,

        TH_DM_FollowConfig = 10,
        PercentTH_FollowConfig = 11,
        TH_TC_FollowConfig = 13,
        TH_Err_FollowConfig = 14,
        TH_DM_OnDay = 15,
        TH_TC_OnDay = 16,
        TH_Error_OnDay = 17,
    }
}