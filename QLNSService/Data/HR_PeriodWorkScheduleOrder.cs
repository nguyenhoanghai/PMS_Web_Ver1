//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNSService.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class HR_PeriodWorkScheduleOrder
    {
        public int PeriodWorkScheduleId { get; set; }
        public int OrderWorkSchedule { get; set; }
        public Nullable<int> DailyWorkScheduleId { get; set; }
        public int CompanyId { get; set; }
    
        public virtual HR_DailyWorkSchedule HR_DailyWorkSchedule { get; set; }
        public virtual HR_PeriodWorkSchedule HR_PeriodWorkSchedule { get; set; }
    }
}
