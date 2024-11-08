using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.Enum
{
    public enum eStatus
    {
        OnCreate = 5,
        Begin_Yet = 1,
        Processing = 2,
        Error = 3,
        Complete = 4,
        DisapproveDoAgain = 6,
        Pause = 7,
        Cancel = 8,
    }
}