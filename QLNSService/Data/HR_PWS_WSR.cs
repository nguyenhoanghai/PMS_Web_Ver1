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
    
    public partial class HR_PWS_WSR
    {
        public int WorkScheduleId { get; set; }
        public int PeriodWorkScheduleId { get; set; }
        public System.DateTime RefDate { get; set; }
        public Nullable<int> RefPWSOrder { get; set; }
        public int CompanyId { get; set; }
    
        public virtual HR_PeriodWorkSchedule HR_PeriodWorkSchedule { get; set; }
        public virtual HR_WorkScheduleRule HR_WorkScheduleRule { get; set; }
    }
}
